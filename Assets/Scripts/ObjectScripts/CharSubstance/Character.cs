using System;
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
        public Properties Properties;
        public int RaceIndex = 0;
        
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

        public override float Attacked(float damage, BodyPart bodyPart, float defenceRatio = 1)
        {
            if (!bodyPart.Available)
            {
                return 0;
            }
            damage = base.Attacked(damage, bodyPart, defenceRatio);
            if (!bodyPart.Available)
            {
                DropFetchObject(bodyPart);
                if (bodyPart.AttachBodyPart != string.Empty)
                {
                    DropFetchObject(BodyParts[bodyPart.AttachBodyPart]);
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
//            if (check && !MoveCheck(delta))
//            {
//                return false;
//            }

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


        public virtual void RefreshProperties()
        {
            Properties.RefreshProperties();
            if (Dead && IsTurn())
            {
                foreach (var part in BodyParts.Values)
                {
                    if (!part.Available) continue;
                    Attacked(1, part, 0);
                }

                ActivateTime += 1000;
                return;
            }

            if (Health >= Properties.GetMaxHealth())
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
            GameSetting.Instance.RaceList[RaceIndex].RefactorGameObject(this);
            ActivateTime = SceneManager.Instance.CurrentTime;
            base.Initialize(worldCoord, areaIdentity);
            foreach (var bodyPart in BodyParts.Values)
            {
                if (bodyPart.Fetchable)
                {
                    FetchDictionary.Add(bodyPart, null);
                }
            }
            RefreshProperties();
        }

        public Dictionary<BodyPart, BaseObject> FetchDictionary = new Dictionary<BodyPart, BaseObject>();

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