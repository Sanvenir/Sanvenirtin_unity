using UnityEngine;

namespace ObjectScripts.SpriteController
{
    public class EffectController : MonoBehaviour
    {
        public Sprite[] Sprites;
        [HideInInspector] public BaseObject Parent;
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        ///     If Disappear Time is 0, the effect last unlimited
        /// </summary>
        public int DisappearTime;

        public bool IsLoop;

        private int _index;

        public void Initialize(int lastTime = 0)
        {
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

            if (++_index == Sprites.Length)
            {
                if (!IsLoop)
                {
                    Destroy(gameObject);
                    return;
                }

                _index = 0;
            }

            _spriteRenderer.sprite = Sprites[_index];
        }
    }
}