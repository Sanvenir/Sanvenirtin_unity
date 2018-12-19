using ObjectScripts.SpriteController;
using UnityEngine;

namespace ObjectScripts
{
    public abstract class BaseObject: MonoBehaviour
    {
        public string Name;
        
        [HideInInspector]
        public ISubstanceSpriteController SubstanceSpriteController;
        protected SpriteRenderer SpriteRenderer;
        protected Collider2D Collider2D;
        
        public abstract float GetSize();
        public abstract float GetWeight();

        protected void Initialize()
        {
            SubstanceSpriteController = GetComponent<ISubstanceSpriteController>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Collider2D = GetComponent<Collider2D>();
        }
    }
}