using System.Text;
using ObjectScripts.SpriteController;
using UnityEngine;

namespace ObjectScripts
{
    public abstract class BaseObject: MonoBehaviour
    {
        public string Name;
        public StringBuilder Describe;

        public SpriteRenderer SpriteRenderer;
        
        public Collider2D Collider2D;
        
        public abstract float GetSize();
        public abstract float GetWeight();

        protected void Initialize()
        {
        }
    }
}