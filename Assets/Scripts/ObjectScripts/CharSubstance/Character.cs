using System;
using System.Collections;
using System.Collections.Generic;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.RaceScripts;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharSubstance
{
    public abstract class Character : ComplexObject
    {
        public const float DropIncrement = 0.1f;
        public CharacterController.CharacterController Controller;
        public Properties Properties;
        public int RaceIndex = 0;

        // Key is all known character
        [HideInInspector] public HashSet<Character> VisibleCharacters;
        
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
            if (!bodyPart.Available)
            {
                return 0;
            }
            var intensity = base.Attacked(damage, bodyPart);
            if (!bodyPart.Available)
            {
                DropFetchObject(bodyPart);
                if (bodyPart.AttachBodyPart != string.Empty)
                {
                    DropFetchObject(BodyParts[bodyPart.AttachBodyPart]);
                }
                
                if (bodyPart.Essential)
                {
                    Die();
                }
            }
            Sanity += intensity / Properties.WillPower;
            return intensity;
        }

        public bool MoveCheck<T>(Vector2Int delta, out T collide)
            where T : ComplexObject
        {
            Collider2D.offset = delta;
            var result = CheckCollider<T>(out collide);
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

        protected IEnumerator SmoothMovement(Vector3 end, int moveTime)
        {
            var moveSteps = moveTime / SceneManager.Instance.GetUpdateTime();
            var moveVector = (end - transform.position) / moveSteps;
            for (; moveSteps != 0; moveSteps--)
            {
                SpriteController.StartMoving();
                transform.position += moveVector;
                SpriteRenderer.sortingOrder = -Utils.FloatToInt(transform.position.y);
                Collider2D.offset = end - transform.position;
                yield return null;
            }

            SpriteController.StopMoving();
        }

        public void TurnTo(Direction direction)
        {
            SpriteController.SetDirection(direction);
        }

        public void AttackMovement(Direction direction, int attTime, float intense=0.5f)
        {
            var delta = (Vector2)Utils.DirectionToVector(direction) * intense;
            SpriteController.SetDirection(direction);
            transform.position = delta + WorldPos;
            Collider2D.offset = -delta;
            StartCoroutine(SmoothMovement(WorldPos, attTime));
        }

        protected override void Update()
        {
            base.Update();
            SpriteRenderer.enabled = 
                Dead || SceneManager.Instance.PlayerObject.VisibleCharacters.Contains(this);
        }


        public virtual void RefreshProperties()
        {
            Properties.RefreshProperties();
            
            VisibleCharacters = new HashSet<Character>();
            var hits = Physics2D.OverlapCircleAll(WorldPos, Properties.Perception,
                SceneManager.Instance.BlockFilter.layerMask);
            foreach (var hit in hits)
            {
                var character = hit.GetComponent<Character>();
                if (!character) continue;
                VisibleCharacters.Add(character);
            }
            
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
            transform.position += new Vector3(
                (float) Utils.ProcessRandom.NextDouble() * DropIncrement * 2 - DropIncrement,
                (float) Utils.ProcessRandom.NextDouble() * DropIncrement * 2 - DropIncrement);
            SceneManager.Instance.Print(
                GameText.Instance.GetCharacterDeadLog(TextName));
            SetDisable();
        }

        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            // GameSetting.Instance.RaceList[RaceIndex].RefactorGameObject(this);
            ActivateTime = SceneManager.Instance.CurrentTime;
            
            BodyParts = new Dictionary<string, BodyPart>();
            foreach (var bodyPart in Properties.BodyParts)
            {
                BodyParts.Add(bodyPart.Name, (BodyPart) bodyPart.Clone());
            }

            foreach (var bodyPart in BodyParts.Values)
            {
                if (bodyPart.Fetchable)
                {
                    FetchDictionary.Add(bodyPart, null);
                }
            }
            base.Initialize(worldCoord, areaIdentity);
            RefreshProperties();
        }

        public Dictionary<BodyPart, BaseObject> FetchDictionary = new Dictionary<BodyPart, BaseObject>();

        public IEnumerable<BodyPart> GetFreeFetchParts()
        {
            foreach (var item in FetchDictionary)
            {
                if (item.Value == null)
                {
                    yield return item.Key;
                }
            }
        }

        [HideInInspector]
        public int ActivateTime;
        public EffectController AttackActionEffect;
        
        // Properties
        [HideInInspector] public float Health = 0;
        [HideInInspector] public float Sanity = 0;
        [HideInInspector] public float Endure = 0;
        [HideInInspector] public float Hunger = 0;
        [HideInInspector] public float Marady = 0; // Similar to Mana
        public int Age;
        public Gender Gender;

        [HideInInspector]
        public bool Dead = false;

        public virtual void Recovering()
        {
            var endureRec = Mathf.Min(Properties.GetEndureRecover(), Endure);
            Hunger += endureRec / 100f + Properties.GetBaseRecover() * 10f;
            Endure -= endureRec;
            Health -= Properties.GetHealthRecover();
            if (Health < 0) Health = 0;
            foreach (var part in BodyParts.Values)
            {
                if(!part.Available) continue;
                part.HitPoint.Value += Properties.GetHealthRecover();
            }
        }
    }
}