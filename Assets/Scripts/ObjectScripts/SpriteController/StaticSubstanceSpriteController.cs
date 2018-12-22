using UnityEngine;
using UtilScripts;

namespace ObjectScripts.SpriteController
{
    public class StaticSubstanceSpriteController : MonoBehaviour, ISubstanceSpriteController
    {
        public Sprite[] Sprites;
        public Sprite DisabledSprite;

        public float Speed = 3;
        private bool _disabled = false;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = Sprites[0];
            if (DisabledSprite == null)
            {
                DisabledSprite = Sprites[0];
            }
        }

        public void SetDirection(Direction direction)
        {
        }

        public bool IsMoving()
        {
            return false;
        }

        public void StartMoving()
        {
        }

        public void StopMoving()
        {
        }

        public void SetDisable(bool disabled)
        {
            _disabled = disabled;
        }

        private void Update()
        {
            if (_disabled)
            {
                if (DisabledSprite == null) return;
                _spriteRenderer.sprite = DisabledSprite;
                return;
            }

            var timeIndex = (int) (Time.time * Speed);
            var index = timeIndex % Sprites.Length;
            _spriteRenderer.sprite = Sprites[index];
        }
    }
}