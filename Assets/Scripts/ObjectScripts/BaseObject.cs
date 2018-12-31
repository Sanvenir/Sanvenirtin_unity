using System;
using System.Collections.Generic;
using System.Text;
using ObjectScripts.ActionScripts;
using ObjectScripts.SpriteController;
using UnityEngine;

namespace ObjectScripts
{
    public abstract class BaseObject: MonoBehaviour
    {
        public string Name;
        public StringBuilder Info = new StringBuilder();

        public SpriteRenderer SpriteRenderer;
        
        public Collider2D Collider2D;
        
        public abstract float GetSize();
        public abstract float GetWeight();

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