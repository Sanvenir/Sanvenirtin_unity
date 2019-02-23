using System;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.SpriteController
{
    public class EffectController : MonoBehaviour
    {
        public Sprite[] Sprites;
        [HideInInspector] public BaseObject Parent;
        private SpriteRenderer _spriteRenderer;
        private Direction _direction;

        /// <summary>
        ///     If Disappear Time is 0, the effect last unlimited
        /// </summary>
        public int DisappearTime;

        public bool IsLoop;
        public int Speed = 3;

        private int _index;

        public void Initialize(int lastTime = 0, Direction direction = Direction.None)
        {
            _direction = direction;
            switch (direction)
            {
                case Direction.Down:
                    transform.Rotate(0, 0, 270);
                    break;
                case Direction.Left:
                    transform.Rotate(0, 0, 180);
                    break;
                case Direction.Up:
                    transform.Rotate(0, 0, 90);
                    break;
                case Direction.Right:
                    transform.Rotate(0, 0, 0);
                    break;
                case Direction.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
            if (lastTime == 0)
            {
                IsLoop = false;
            }

            else
            {
                IsLoop = true;
                DisappearTime = SceneManager.Instance.CurrentTime + lastTime;
            }
        }

        public void AddTime(int lastTime)
        {
            DisappearTime += lastTime;
        }

        public void SetTime(int lastTime)
        {
            DisappearTime = SceneManager.Instance.CurrentTime + lastTime;
        }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _index = 0;
            _spriteRenderer.sprite = Sprites[0];
        }

        private void Update()
        {
            if (Parent != null) _spriteRenderer.enabled = Parent.Visible;
            if (IsLoop && DisappearTime != 0 && SceneManager.Instance.CurrentTime > DisappearTime)
            {
                Destroy(gameObject);
                return;
            }

            if (++_index / Speed == Sprites.Length)
            {
                if (!IsLoop)
                {
                    Destroy(gameObject);
                    return;
                }

                _index = 0;
            }

            _spriteRenderer.sprite = Sprites[_index / Speed];
        }
    }
}