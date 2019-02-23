using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class SelectionButton : MonoBehaviour
    {
        private float _currentAlpha;
        public Image ButtonImage;
        public Text ButtonText;
        public Color ImageColor = new Color(0, 1, 1, 0.5f);

        [HideInInspector] public float TargetAlpha;

        [HideInInspector] public Vector2 TargetPos;

        [HideInInspector] public float TargetScale;

        public Color TextColor = Color.black;

        private void Awake()
        {
            _currentAlpha = 0;
            SetAlpha(_currentAlpha);
        }

        public void SetText(string text)
        {
            ButtonText.text = text;
        }

        private void SetAlpha(float alpha)
        {
            ButtonImage.color = ImageColor * alpha;
            ButtonText.color = TextColor * alpha;
        }

        private void Update()
        {
            transform.position = TargetPos * 0.1f + (Vector2) transform.position * 0.9f;
            transform.localScale = Vector3.one * TargetScale * 0.1f +
                                   transform.localScale * 0.9f;
            _currentAlpha = TargetAlpha * 0.1f + _currentAlpha * 0.9f;
            SetAlpha(_currentAlpha);
        }
    }
}