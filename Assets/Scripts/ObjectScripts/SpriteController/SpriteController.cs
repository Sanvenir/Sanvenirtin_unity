using UnityEngine;
using UtilScripts;

namespace ObjectScripts.SpriteController
{
    public abstract class SpriteController : MonoBehaviour
    {
        public Sprite DisabledSprite;
        public float Speed = 3;
        [HideInInspector] public SpriteRenderer SpriteRenderer;

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public abstract void SetDirection(Direction direction);
        public abstract void StartMoving();
        public abstract void StopMoving();
        public abstract void SetDisable(bool disabled);

        /// <summary>
        ///     Add new sprite to current sprite controller
        /// </summary>
        /// <param name="index"></param>
        /// <param name="baseObject"></param>
        public abstract void AddNewChildSprite(string index, BaseObject baseObject);

        /// <summary>
        ///     Remove sprite at index
        /// </summary>
        /// <param name="index"></param>
        public abstract void RemoveChildSprite(string index);
    }
}