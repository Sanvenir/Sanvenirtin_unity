using System.Collections;
using System.Collections.Generic;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharSubstance
{
    public abstract class Character : Substance
    {
        public CharacterController.CharacterController Controller;

        public bool IsTurn()
        {
            return SceneManager.Instance.CurrentTime >= ActivateTime;
        }

        public override float Attacked(float damage, BodyPart bodyPart, float defenceRatio = 1)
        {
            damage = base.Attacked(damage, bodyPart, defenceRatio);
            if (!bodyPart.Available)
            {
                if (FetchDictionary.ContainsKey(bodyPart) && 
                    FetchDictionary[bodyPart] != null)
                {
                    FetchDictionary[bodyPart].gameObject.SetActive(true);
                    FetchDictionary[bodyPart].transform.position =
                        WorldPos + new Vector2(
                            (float) Utils.ProcessRandom.NextDouble() - 0.5f,
                            (float) Utils.ProcessRandom.NextDouble() - 0.5f);
                }
                if (bodyPart.AttachBodyPart != null && 
                    FetchDictionary.ContainsKey(bodyPart.AttachBodyPart) && 
                    FetchDictionary[bodyPart.AttachBodyPart] != null)
                {
                    FetchDictionary[bodyPart.AttachBodyPart].gameObject.SetActive(true);
                    FetchDictionary[bodyPart.AttachBodyPart].transform.position =
                        WorldPos + new Vector2(
                            (float) Utils.ProcessRandom.NextDouble() - 0.5f,
                            (float) Utils.ProcessRandom.NextDouble() - 0.5f);
                }
                if (bodyPart.Essential)
                {
                    Dead = true;
                }
            }
            Health += damage;
            return damage;
        }

        public bool MoveCheck<T>(Vector2Int delta, out T collide)
            where T : Substance
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
        
        public bool Move(Vector2Int delta, int moveTime, bool check = true)
        {
            if (delta == Vector2Int.zero) return true;
            var endPos = WorldPos + delta;
            SpriteController.SetDirection(Utils.VectorToDirection(delta));
            if (check && !MoveCheck(delta))
            {
                return false;
            }

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


        public virtual void RefreshProperties()
        {
            _reactTime = Mathf.Max(1, (int) (100.0f / Mathf.Log(1f + Speed)));
            _actRatio = Mathf.Max(1, (int) (10 * Mathf.Log(Speed, 10 + ActSpeed)));
            _moveRatio = Mathf.Max(1, (int) (10 * Mathf.Log(Speed, 10 + MoveSpeed)));
            if (Dead && IsTurn())
            {
                foreach (var part in GetAllBodyParts())
                {
                    if (!part.Available) continue;
                    Attacked(1, part, 0);
                }

                ActivateTime += 1000;
                return;
            }

            if (Health >= GetMaxHealth())
            {
                Dead = true;
            }

            if (Dead)
            {
                SetDisable();   
            }
        }

        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            base.Initialize(worldCoord, areaIdentity);
            ActivateTime = SceneManager.Instance.CurrentTime;
            RefreshProperties();
        }

        public Dictionary<BodyPart, BaseObject> FetchDictionary = new Dictionary<BodyPart, BaseObject>();

        [HideInInspector]
        public int ActivateTime;
        public EffectController AttackActionEffect;
        
        // Properties
        [HideInInspector]
        public bool Dead = false;
        [HideInInspector]
        public float Health = 0;
        [HideInInspector]
        public float Sanity = 0;
        [HideInInspector]
        public float Endure = 0;
        [HideInInspector]
        public float Hunger = 0;
        [HideInInspector]
        public float Marady = 0;  // Similar to Mana

        public float Speed = 10;
        public float MoveSpeed = 10;
        public float ActSpeed = 10;

        public float Strength = 10;
        public float Dexterity = 10;
        public float Constitution = 10;
        public float Metabolism = 10;

        public float WillPower = 10;
        public float Intelligence = 10;
        public float Perception = 10;

        // Get Methods
        private int _reactTime;

        public virtual int GetReactTime()
        {
            return _reactTime;
        }

        private int _actRatio;

        public virtual int GetActRatio()
        {
            return _actRatio;
        }

        private int _moveRatio;

        public virtual int GetMoveRatio()
        {
            return _moveRatio;
        }

        public virtual int GetMoveTime()
        {
            return GetMoveRatio() * GetReactTime();
        }

        public virtual int GetActTime()
        {
            return GetActRatio() * GetReactTime();
        }

        public virtual float GetMaxHealth()
        {
            return Constitution * 10;
        }

        public virtual float GetMaxSanity()
        {
            return WillPower * 10;
        }

        public virtual float GetMaxEndure()
        {
            return Constitution * 10;
        }

        public virtual float GetMaxHunger()
        {
            return 200;
        }

        public virtual float GetBaseAttack()
        {
            return Strength;
        }

        public virtual float GetBaseRecover()
        {
            return Metabolism / 10000f;
        }

        public virtual float GetHealthRecover()
        {
            return Mathf.Min((GetBaseRecover() * 10f), Health);
        }
        
        public virtual float GetEndureRecover()
        {
            return Mathf.Min((GetMaxEndure() * GetBaseRecover()), Endure);
        }
        
        public virtual void Recovering()
        {
            var endureRec = GetEndureRecover();
            Hunger += endureRec / 100f + GetBaseRecover() * 10f;
            Endure -= endureRec;
            Health -= GetHealthRecover();
            foreach (var part in GetAllBodyParts())
            {
                if(!part.Available) continue;
                part.HitPoint.Value += GetHealthRecover();
            }
        }
    }
}