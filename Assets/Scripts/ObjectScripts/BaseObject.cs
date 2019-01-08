using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ObjectScripts.ActionScripts;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts
{
    /// <inheritdoc />
    /// <summary>
    /// Base type of Game Object, for every game entity which has a name, a image(SpriteRenderer) and checkable collider. 
    /// </summary>
    public abstract class BaseObject: MonoBehaviour
    {
        public string TextName;
        public string Info;
        [HideInInspector] public Vector2 WorldPos;
        [HideInInspector] public Vector2Int WorldCoord;

        public SpriteRenderer SpriteRenderer;
        public SpriteController.SpriteController SpriteController;
        
        /// <summary>
        /// The checkable collider of object, if it is a substance, the layer should be BlockLayer
        /// </summary>
        public Collider2D Collider2D;
        
        /// <summary>
        /// Get the size of this object
        /// </summary>
        /// <returns></returns>
        public abstract float GetSize();
        
        /// <summary>
        /// Get the weight of this object
        /// </summary>
        /// <returns></returns>
        public abstract float GetWeight();

        /// <summary>
        /// After instantiate or enable an object, initialize function needs to be called
        /// </summary>
        public void Initialize()
        {
        }

        public void SetPosition(Vector2 worldPos)
        {
            transform.position = worldPos;
            WorldPos = SceneManager.Instance.NormalizeWorldPos(worldPos);
            WorldCoord = SceneManager.Instance.WorldPosToCoord(worldPos);
        }

        protected IEnumerator SmoothMovement(Vector3 end, int moveTime)
        {
            WorldPos = end;
            WorldCoord = SceneManager.Instance.WorldPosToCoord(WorldPos);
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
        
        private void LateUpdate()
        {
            if ((SceneManager.Instance.Player.transform.position -
                 transform.position).sqrMagnitude > 5000)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void Update()
        {
            SpriteRenderer.enabled = SceneManager.Instance.PlayerObject.IsVisible(this);
        }
    }
}