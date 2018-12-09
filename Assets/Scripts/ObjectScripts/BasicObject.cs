using ObjectScripts.SpriteController;
using UnityEngine;

namespace ObjectScripts
{
    public class BasicObject: MonoBehaviour
    {
        [HideInInspector]
        public ISubstanceSpriteController SubstanceSpriteController;
        protected SpriteRenderer SpriteRenderer;
        protected Collider2D Collider2D;

        protected void Initialize()
        {
            SubstanceSpriteController = GetComponent<ISubstanceSpriteController>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Collider2D = GetComponent<Collider2D>();
        }
    }
}