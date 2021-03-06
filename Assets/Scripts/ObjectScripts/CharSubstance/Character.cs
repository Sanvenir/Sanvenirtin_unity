using System;
using System.Collections.Generic;
using AreaScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.SpriteController;
using ObjectScripts.StyleScripts.ActStyleScripts;
using ObjectScripts.StyleScripts.MaradyStyleScripts;
using ObjectScripts.StyleScripts.MentalStyleScripts;
using ObjectScripts.StyleScripts.MoveStyleScripts;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharSubstance
{
    public abstract class Character : Substance
    {
        private float _endure;

        // Properties
        private float _health;

        private float _hunger;

        private float _sanity;

        [HideInInspector] public int ActivateTime;

        public List<BaseActStyle> ActStyleList;

        public int Age;
        public EffectController AttackedEffect;
        public CharacterController.CharacterController Controller;
        [HideInInspector] public BaseActStyle CurrentActStyle;
        [HideInInspector] public BaseMaradyStyle CurrentMaradyStyle;
        [HideInInspector] public BaseMentalStyle CurrentMentalStyle;
        [HideInInspector] public BaseMoveStyle CurrentMoveStyle;

        [HideInInspector] public bool Dead;

        public Dictionary<BodyPart, BaseObject> FetchDictionary = new Dictionary<BodyPart, BaseObject>();
        public Gender Gender;

        public List<BaseMaradyStyle> MaradyStyleList;

        public List<BaseMentalStyle> MentalStyleList;

        public List<BaseMoveStyle> MoveStyleList;
        public Properties Properties;
        public int RaceIndex;

        /// <summary>
        ///     Parent may falls into stun after sanity reach the max value
        ///     Probability: (Sanity - MaxSanity) / MaxSanity
        ///     After sanity return to
        /// </summary>
        public Effect Stun;

        public float Health
        {
            get { return _health; }
            set { _health = Math.Max(0, value); }
        }

        public float Sanity
        {
            get { return _sanity; }
            set
            {
                if (Stun.Value && value > _sanity) return;
                _sanity = Math.Max(0, value);
            }
        }

        public float Endure
        {
            get { return _endure; }
            set
            {
                // Use endure greater than max endure cause sanity damage but also improve will power
                if (value > Properties.GetMaxEndure(0) && value > _endure)
                {
                    var sanityCost = Endure - Properties.GetMaxEndure(0);
                    Sanity += sanityCost;
                    Properties.WillPower.Use(Mathf.Min(1f, sanityCost / Properties.GetMaxEndure(0)));
                }

                _endure = Math.Max(0, value);
            }
        }

        public float Hunger
        {
            get { return _hunger; }
            set { _hunger = Mathf.Min(Properties.GetMaxHunger(0) * 2, value); }
        }

        public float Marady { get; set; }

        public bool IsTurn()
        {
            return SceneManager.Instance.CurrentTime >= ActivateTime;
        }

        public void DropFetchObject(BodyPart bodyPart)
        {
            if (!FetchDictionary.ContainsKey(bodyPart)) return;
            if (FetchDictionary[bodyPart] == null) return;

            FetchDictionary[bodyPart].gameObject.SetActive(true);
            FetchDictionary[bodyPart].transform.position = Utils.GetRandomShiftPosition(WorldPos, 0.1f);
            FetchDictionary[bodyPart] = null;
            SpriteController.RemoveChildSprite(bodyPart.Name);
        }

        public override float Attacked(DamageValue damage, BodyPart bodyPart)
        {
            if (!bodyPart.Available) return 0;
            var intensity = base.Attacked(damage, bodyPart);
            if (!bodyPart.Available)
            {
                SceneManager.Instance.Print(
                    GameText.Instance.GetBodyPartDestroyLog(
                        TextName, bodyPart.TextName), WorldCoord);
                DropFetchObject(bodyPart);
                if (!string.IsNullOrEmpty(bodyPart.AttachBodyPart)) DropFetchObject(BodyParts[bodyPart.AttachBodyPart]);
                Health += bodyPart.Health;
                Sanity += bodyPart.Health;
            }

            Health += intensity / 10;
            if (!Stun.Value) Sanity += intensity;
            PlayEffect(AttackedEffect);
            return intensity;
        }

        public bool MoveCheck<T>(Vector2Int delta, out T collide)
            where T : Substance
        {
            Collider2D.offset = delta;
            var result = CheckCollider(out collide);
            Collider2D.offset = Vector2.zero;
            return result && SceneManager.Instance.GetTileType(WorldCoord + delta) != TileType.Block;
        }

        public bool MoveCheck(Vector2Int delta)
        {
            Collider2D.offset = delta;
            var result = CheckCollider();
            Collider2D.offset = Vector2.zero;
            return result && SceneManager.Instance.GetTileType(WorldCoord + delta) != TileType.Block;
        }

        public bool Move(Vector2Int delta, int moveTime)
        {
            if (delta == Vector2Int.zero) return true;
            var endPos = WorldPos + delta;
            SpriteController.SetDirection(Utils.VectorToDirection(delta));
            if (!MoveCheck(delta)) return false;
            SmoothMoveTo(endPos, moveTime);
            SpriteRenderer.color = SceneManager.Instance.GetTileType(WorldCoord) == TileType.Water
                ? Color.cyan
                : Color.white;
            return true;
        }

        public void TurnTo(Direction direction)
        {
            SpriteController.SetDirection(direction);
        }

        public void AttackMovement(Direction direction, int attTime, float intense = 0.5f)
        {
            var delta = (Vector2) Utils.DirectionToVector(direction) * intense;
            SpriteController.SetDirection(direction);
            HitTo(direction, attTime);
        }

        public bool CheckInteractRange(Vector2 pos)
        {
            return (pos - WorldPos).sqrMagnitude < Properties.InteractRangeSqr();
        }

        protected override void Update()
        {
            base.Update();
        }


        public bool IsAudible(Vector2Int coord)
        {
            return (coord - WorldCoord).sqrMagnitude < Properties.GetVisibleRangeSqr(0);
        }

        public bool IsVisible(Vector2Int coord)
        {
            var distance = (coord - WorldCoord).sqrMagnitude;
            if (distance < Properties.GetSensibleRangeSqr(0)) return true;
            var pos = SceneManager.Instance.WorldCoordToPos(coord);
            var hit = Physics2D.OverlapPoint(pos, SceneManager.Instance.BlockInspectLayer);
            if (hit != null) hit.enabled = false;
            Collider2D.enabled = false;
            var result = Physics2D.Linecast(transform.position, SceneManager.Instance.WorldCoordToPos(coord),
                                 SceneManager.Instance.BlockInspectLayer)
                             .collider == null;
            Collider2D.enabled = true;
            if (hit != null) hit.enabled = true;
            return result && distance < Properties.GetVisibleRangeSqr(0);
        }

        public bool IsVisible(Collider2D hit)
        {
            var distance = (hit.transform.position - transform.position).sqrMagnitude;
            if (distance < Properties.GetSensibleRangeSqr(0)) return true;
            Collider2D.enabled = false;
            hit.enabled = false;
            var result = Physics2D.Linecast(transform.position, hit.transform.position,
                                 SceneManager.Instance.BlockInspectLayer)
                             .collider == null;
            Collider2D.enabled = true;
            hit.enabled = true;
            return result && distance < Properties.GetVisibleRangeSqr(0);
        }


        public bool IsVisible(BaseObject baseObject)
        {
            var distance = (baseObject.transform.position - transform.position).sqrMagnitude;
            if (distance < Properties.GetSensibleRangeSqr(0)) return true;
            Collider2D.enabled = false;
            baseObject.Collider2D.enabled = false;
            var result =
                Physics2D.Linecast(WorldPos, baseObject.transform.position, SceneManager.Instance.BlockInspectLayer)
                    .collider == null;
            Collider2D.enabled = true;
            baseObject.Collider2D.enabled = true;
            return result && distance < Properties.GetVisibleRangeSqr(0);
            ;
        }

        public IEnumerable<T> GetVisibleObjects<T>()
            where T : BaseObject
        {
            var hits = Physics2D.OverlapCircleAll(WorldPos, Properties.GetVisibleRange(0),
                SceneManager.Instance.ObjectLayer);
            foreach (var hit in hits)
            {
                if (!IsVisible(hit)) continue;
                var baseObject = hit.GetComponent<T>();
                if (baseObject == null || baseObject == this) continue;
                yield return baseObject;
            }
        }

        public virtual void RefreshProperties()
        {
            Properties.RefreshProperties();

            if (Dead)
            {
                foreach (var part in BodyParts.Values)
                {
                    if (!part.Available) continue;
                    part.DoPenetrateHitDamage(1f);
                }

                ActivateTime += 1000;
                return;
            }

            #region CheckProperty

            if (Hunger > Properties.GetMaxHunger(0))
                Health += (Hunger - Properties.GetMaxHunger(0)) / Properties.GetMaxHunger(0);

            if (!Stun.Value && Sanity > Properties.GetMaxSanity(0))
                if (Utils.ProcessRandom.Next((int) Properties.GetMaxSanity(0)) < Sanity - Properties.GetMaxSanity(0))
                {
                    Stun.Enable(this);
                    SceneManager.Instance.Print(
                        GameText.Instance.GetFallIntoStunLog(TextName), WorldCoord);
                }

            if (Health >= Properties.GetMaxHealth(0)) Die();

            #endregion
        }

        public void Die()
        {
            if (Dead) return;
            Dead = true;
            SetPosition(Utils.GetRandomShiftPosition(WorldPos));
            SceneManager.Instance.Print(
                GameText.Instance.GetCharacterDeadLog(TextName), WorldCoord);
            SpriteController.SetDisable(true);
            gameObject.layer = LayerMask.NameToLayer("DisabledLayer");
            Stun.Disable(this);
        }

        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            ActivateTime = SceneManager.Instance.CurrentTime;
            BodyParts = new Dictionary<string, BodyPart>();
            foreach (var bodyPart in Properties.BodyParts)
                BodyParts.Add(bodyPart.Name, (BodyPart) bodyPart.Create(this));
            foreach (var bodyPart in BodyParts.Values)
                if (bodyPart.Fetchable)
                    FetchDictionary.Add(bodyPart, null);

            MoveStyleList = new List<BaseMoveStyle>
            {
                new WalkMoveStyle(this),
                new RunMoveStyle(this),
                new DashMoveStyle(this)
            };

            CurrentMoveStyle = MoveStyleList[0];

            base.Initialize(worldCoord, areaIdentity);
            RefreshProperties();
        }

        public IEnumerable<INamed> GetFreeFetchParts()
        {
            foreach (var item in FetchDictionary)
                if (item.Value == null && item.Key.Available)
                    yield return item.Key;
        }

        public bool CheckEndure()
        {
            if (Endure < Properties.GetMaxEndure(0)) return true;
            return Utils.ProcessRandom.Next((int) Endure) < Properties.GetMaxEndure(0);
        }

        public virtual void Recovering(float intense = 1.0f)
        {
            if (Endure > float.Epsilon)
            {
                var endureRec = Mathf.Min(Properties.GetEndureRecover(0.1f) * intense, Endure);
                Hunger += endureRec / 100f;
                Endure -= endureRec;
            }

            if (Health > float.Epsilon) Health -= Properties.GetHealthRecover(0.1f) * intense;

            if (Sanity > float.Epsilon)
            {
                Sanity -= Properties.GetWillRecover(0.1f) * intense;
                if (Stun.Value)
                    if (Utils.ProcessRandom.Next((int) Properties.GetMaxSanity(0)) > Sanity)
                    {
                        Stun.Disable(this);
                        SceneManager.Instance.Print(
                            GameText.Instance.GetRecoverFromStunLog(TextName), WorldCoord);
                    }
            }

            foreach (var part in BodyParts.Values)
            {
                if (!part.Available) continue;
                part.HitPoint.Value += Properties.GetHealthRecover(0) * intense;
            }
        }

        [Serializable]
        public struct Effect
        {
            public bool Value;

            public void Enable(Character self)
            {
                Value = true;
                _instance = Instantiate(Prefab, self.transform);
                _instance.Parent = self;
            }

            public void Disable(Character self)
            {
                Value = false;
                if (_instance == null) return;
                Destroy(_instance.gameObject);
            }

            public EffectController Prefab;
            private EffectController _instance;
        }
    }
}