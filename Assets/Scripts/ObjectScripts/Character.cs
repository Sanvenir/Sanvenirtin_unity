using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AreaScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts
{
    public abstract class Character : Substance
    {
        [HideInInspector]
        public CharacterController.CharacterController Controller;

        public bool IsTurn()
        {
            return SceneManager.Instance.CurrentTime >= ActivateTime;
        }

        public override float Attacked(float damage, BodyPart bodyPart, float defenceRatio = 1)
        {
            damage = base.Attacked(damage, bodyPart, defenceRatio);
            if (bodyPart.Essential && !bodyPart.Available)
            {
                Dead = true;
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
            SubstanceSpriteController.SetDirection(Utils.VectorToDirection(delta));
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
                SubstanceSpriteController.StartMoving();
                transform.position += moveVector;
                SpriteRenderer.sortingOrder = -Utils.FloatToInt(transform.position.y);
                Collider2D.offset = end - transform.position;
                yield return null;
            }

            SubstanceSpriteController.StopMoving();
        }

        public void TurnTo(Direction direction)
        {
            SubstanceSpriteController.SetDirection(direction);
        }

        public void AttackMovement(Direction direction, int attTime, float intense=0.5f)
        {
            var delta = (Vector2)Utils.DirectionToVector(direction) * intense;
            SubstanceSpriteController.SetDirection(direction);
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
                SubstanceSpriteController.SetDisable(true);   
            }
        }

        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            base.Initialize(worldCoord, areaIdentity);
            ActivateTime = SceneManager.Instance.CurrentTime;
            RefreshProperties();
            Controller = GetComponent<CharacterController.CharacterController>();
        }

        public int ActivateTime;
        public EffectController AttackActionEffect;
        
        // Properties
        public bool Dead = false;
        public float Health = 0;
        public float Sanity = 0;
        public float Endure = 0;
        public float Hunger = 0;
        public float Marady = 0;  // Similar to Mana

        public float Speed = 100;
        public float MoveSpeed = 100;
        public float ActSpeed = 100;

        public float Strength = 100;
        public float Dexterity = 100;
        public float Constitution = 100;
        public float Metabolism = 100;

        public float WillPower = 100;
        public float Intelligence = 100;
        public float Perception = 100;

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
            return Metabolism / 100000f;
        }

        public virtual float GetHealthRecover()
        {
            return Mathf.Min((GetBaseRecover() * 100f), Health);
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
        }
    }
}