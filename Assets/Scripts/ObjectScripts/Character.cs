using System;
using System.Collections;
using AreaScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts
{
    public abstract class Character : Substance
    {

        public bool IsTurn()
        {
            return SceneManager.Instance.CurrentTime >= ActivateTime;
        }

        public override void Attacked(int damage, string partKey, float defenceRatio = 1)
        {
            base.Attacked(damage, partKey, defenceRatio);
            Health += damage;
        }

        public T MoveCheck<T>(Vector2Int delta)
            where T : Substance
        {
            Collider2D.offset = delta;
            var substance = CheckCollider<T>();
            Collider2D.offset = Vector2.zero;
            return substance;
        }
        public bool Move(Vector2Int delta, int moveTime, bool check = true)
        {
            if (delta == Vector2Int.zero) return true;
            var endPos = WorldPos + delta;
            SubstanceSpriteController.SetDirection(Utils.VectorToDirection(delta));
            if (check && MoveCheck<Substance>(delta) != null)
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
                foreach (var key in BodyParts.Keys)
                {
                    // Roting
                    Attacked(1, key, 0);
                }

                ActivateTime += 1000;
                return;
            }

            if (Health >= GetMaxHealth())
            {
                Dead = true;
                SubstanceSpriteController.SetDisable(true);
            }
        }

        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            base.Initialize(worldCoord, areaIdentity);
            ActivateTime = SceneManager.Instance.CurrentTime;
            RefreshProperties();
        }

        public int ActivateTime;
        public EffectController AttackActionEffect;
        
        // Properties
        public bool Dead = false;
        public int Health = 0;
        public int Sanity = 0;
        public int Endure = 0;
        public int Hunger = 0;
        public int Marady = 0;  // Similar to Mana

        public int Speed = 20;
        public int MoveSpeed = 20;
        public int ActSpeed = 20;

        public int Strength = 20;
        public int Dexterity = 20;
        public int Constitution = 20;
        public int Metabolism = 20;

        public int WillPower = 20;
        public int Intelligence = 20;
        public int Perception = 20;

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

        public virtual int GetMaxHealth()
        {
            return Constitution * 10;
        }

        public virtual int GetMaxSanity()
        {
            return WillPower * 10;
        }

        public virtual int GetMaxEndure()
        {
            return Constitution * 10;
        }

        public virtual int GetMaxHunger()
        {
            return 200;
        }

        public virtual int GetBaseAttack()
        {
            return Strength;
        }
    }
}