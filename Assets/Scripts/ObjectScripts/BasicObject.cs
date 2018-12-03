using ObjectScripts.SpriteController;
using UnityEngine;

namespace ObjectScripts
{
    public class BasicObject: MonoBehaviour
    {
        [HideInInspector]
        public ISpriteController SpriteController;
        protected SpriteRenderer SpriteRenderer;
        protected Collider2D Collider2D;

        public BasicObjectProperties BasicObjectProperties;

        protected void Initialize()
        {
            SpriteController = GetComponent<ISpriteController>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Collider2D = GetComponent<Collider2D>();
        }
    }
}