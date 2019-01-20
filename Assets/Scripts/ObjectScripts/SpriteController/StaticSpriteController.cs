using UnityEngine;
using UtilScripts;

namespace ObjectScripts.SpriteController
{
    public class StaticSpriteController : SpriteController
    {
        public Sprite[] Sprites;
        private bool _disabled = false;

        private void Start()
        {
            SpriteRenderer.sprite = Sprites[0];
            if (DisabledSprite == null)
            {
                DisabledSprite = Sprites[0];
            }
        }

        public override void SetDirection(Direction direction)
        {
        }

        public override void StartMoving()
        {
        }

        public override void StopMoving()
        {
        }

        public override void SetDisable(bool disabled)
        {
            _disabled = disabled;
        }

        public override void AddNewChildSprite(string index, BaseObject baseObject)
        {
            return;
        }

        public override void RemoveChildSprite(string index)
        {
            return;
        }

        private void Update()
        {
            if (_disabled)
            {
                if (DisabledSprite == null) return;
                SpriteRenderer.sprite = DisabledSprite;
                return;
            }

            var timeIndex = (int) (Time.time * Speed);
            var index = timeIndex % Sprites.Length;
            SpriteRenderer.sprite = Sprites[index];
        }
    }
}