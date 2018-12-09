using UnityEngine;
using UtilScripts;

namespace ObjectScripts.SpriteController
{
    public class StaticSubstanceSpriteController : MonoBehaviour, ISubstanceSpriteController
    {
        public Sprite[] Sprites;

        public float Speed = 3;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = Sprites[0];
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
        }

        private void Update()
        {
            var timeIndex = (int) (Time.time * Speed);
            var index = timeIndex % Sprites.Length;
            _spriteRenderer.sprite = Sprites[index];
        }
    }
}