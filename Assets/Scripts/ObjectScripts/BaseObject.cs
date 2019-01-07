using System;
using System.Collections.Generic;
using System.Text;
using ObjectScripts.ActionScripts;
using ObjectScripts.SpriteController;
using UnityEngine;

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

        public SpriteRenderer SpriteRenderer;
        
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
        protected void Initialize()
        {
        }

        private void LateUpdate()
        {
            if ((SceneManager.Instance.Player.transform.position -
                 transform.position).sqrMagnitude > 5000)
            {
                Destroy(gameObject);
            }
        }
    }
}