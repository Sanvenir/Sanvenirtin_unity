using System.Collections.Generic;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.SpriteController
{
    public abstract class SpriteController : MonoBehaviour
    {
        [HideInInspector] public SpriteRenderer SpriteRenderer;
        public Sprite DisabledSprite;
        public float Speed = 3;

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public abstract void SetDirection(Direction direction);
        public abstract bool IsMoving();
        public abstract void StartMoving();
        public abstract void StopMoving();
        public abstract void SetDisable(bool disabled);
    }
}