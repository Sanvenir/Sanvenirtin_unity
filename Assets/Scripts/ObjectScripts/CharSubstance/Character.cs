using System.Collections.Generic;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharSubstance
{
    public abstract class Character : Substance
    {
        public CharacterController.CharacterController Controller;
        public Properties Properties;
        public int RaceIndex;

        public bool IsTurn()
        {
            return SceneManager.Instance.CurrentTime >= ActivateTime;
        }

        public void DropFetchObject(BodyPart bodyPart)
        {
            if (!FetchDictionary.ContainsKey(bodyPart)) return;
            if (FetchDictionary[bodyPart] == null) return;

            FetchDictionary[bodyPart].gameObject.SetActive(true);
            FetchDictionary[bodyPart].transform.position =
                WorldPos + new Vector2(
                    (float) Utils.ProcessRandom.NextDouble() - 0.5f,
                    (float) Utils.ProcessRandom.NextDouble() - 0.5f);
            FetchDictionary[bodyPart] = null;
        }

        public override float Attacked(DamageValue damage, BodyPart bodyPart)
        {
            if (!bodyPart.Available) return 0;
            var intensity = base.Attacked(damage, bodyPart);
            if (!bodyPart.Available)
            {
                DropFetchObject(bodyPart);
                if (!string.IsNullOrEmpty(bodyPart.AttachBodyPart)) DropFetchObject(BodyParts[bodyPart.AttachBodyPart]);

                if (bodyPart.Essential) Die();
            }

            Sanity += intensity / Properties.WillPower;
            return intensity;
        }

        public bool MoveCheck<T>(Vector2Int delta, out T collide)
            where T : ComplexObject
        {
            Collider2D.offset = delta;
            var result = CheckCollider(out collide);
            Collider2D.offset = Vector2.zero;
            return result;
        }

        public bool MoveCheck(Vector2Int delta)
        {
            Collider2D.offset = delta;
            var result = CheckCollider();
            Collider2D.offset = Vector2.zero;
            return result;
        }

        public bool Move(Vector2Int delta, int moveTime)
        {
            if (delta == Vector2Int.zero) return true;
            var endPos = WorldPos + delta;
            SpriteController.SetDirection(Utils.VectorToDirection(delta));

            if (!MoveCheck(delta)) return false;

            Collider2D.offset = delta;
            WorldCoord += delta;
            WorldPos = endPos;
            StartCoroutine(SmoothMovement(endPos, moveTime));
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
            transform.position = delta + WorldPos;
            Collider2D.offset = -delta;
            StartCoroutine(SmoothMovement(WorldPos, attTime));
        }

        protected override void Update()
        {
            base.Update();
        }


        public bool IsAudible(Vector2Int coord)
        {
            return (coord - WorldCoord).sqrMagnitude < Properties.GetVisibleRange();
        }
        
        public bool IsVisible(Collider2D hit)
        {
            var distance = (hit.transform.position - transform.position).sqrMagnitude;
            if (distance < Properties.GetSensibleRange()) return true;
            Collider2D.enabled = false;
            hit.enabled = false;
            var result = Physics2D.Linecast(WorldPos, hit.transform.position, SceneManager.Instance.BlockInspectLayer)
                             .collider == null;
            Collider2D.enabled = true;
            hit.enabled = true;
            return result && distance < Properties.GetVisibleRange();
        }

        public IEnumerable<T> GetVisibleObjects<T>()
            where T : BaseObject
        {
            var hits = Physics2D.OverlapCircleAll(WorldPos, Properties.Perception,
                SceneManager.Instance.ObjectLayer);
            foreach (var hit in hits)
            {
                if (!IsVisible(hit)) continue;
                var baseObject = hit.GetComponent<T>();
                if (baseObject == null || baseObject == this) continue;
                yield return baseObject;
            }
        }

        public bool IsVisible(BaseObject baseObject)
        {
            var distance = (baseObject.transform.position - transform.position).sqrMagnitude;
            if (distance < Properties.GetSensibleRange()) return true;
            Collider2D.enabled = false;
            baseObject.Collider2D.enabled = false;
            var result =
                Physics2D.Linecast(WorldPos, baseObject.transform.position, SceneManager.Instance.BlockInspectLayer)
                    .collider == null;
            Collider2D.enabled = true;
            baseObject.Collider2D.enabled = true;
            return result && distance < Properties.GetVisibleRange();
            ;
        }

        public virtual void RefreshProperties()
        {
            Properties.RefreshProperties();

            if (Dead && IsTurn())
            {
                foreach (var part in BodyParts.Values)
                {
                    if (!part.Available) continue;
                    part.DoPenetrateHitDamage(1f);
                }

                ActivateTime += 1000;
                return;
            }

            if (!(Health >= Properties.GetMaxHealth()) || Dead) return;
            Die();
        }

        public void Die()
        {
            if (Dead) return;
            Dead = true;
            SetPosition(Utils.GetRandomShiftPosition(WorldPos));
            SceneManager.Instance.Print(
                GameText.Instance.GetCharacterDeadLog(TextName), WorldCoord);
            SetDisable();
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
            base.Initialize(worldCoord, areaIdentity);
            RefreshProperties();
        }

        public Dictionary<BodyPart, BaseObject> FetchDictionary = new Dictionary<BodyPart, BaseObject>();

        public IEnumerable<BodyPart> GetFreeFetchParts()
        {
            foreach (var item in FetchDictionary)
                if (item.Value == null)
                    yield return item.Key;
        }

        [HideInInspector] public int ActivateTime;
        public EffectController AttackActionEffect;

        // Properties
        [HideInInspector] public float Health;
        [HideInInspector] public float Sanity;
        [HideInInspector] public float Endure;
        [HideInInspector] public float Hunger;
        [HideInInspector] public float Marady; // Similar to Mana
        public int Age;
        public Gender Gender;

        [HideInInspector] public bool Dead;

        public virtual void Recovering()
        {
            var endureRec = Mathf.Min(Properties.GetEndureRecover(), Endure);
            Hunger += endureRec / 100f + Properties.GetBaseRecover() * 10f;
            Endure -= endureRec;
            Health -= Properties.GetHealthRecover();
            if (Health < 0) Health = 0;
            foreach (var part in BodyParts.Values)
            {
                if (!part.Available) continue;
                part.HitPoint.Value += Properties.GetHealthRecover();
            }
        }
    }
}