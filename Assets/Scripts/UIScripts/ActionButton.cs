using ObjectScripts.CharacterController.PlayerOrder;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class ActionButton: MonoBehaviour
    {
        public Image ButtonImage;
        public Text ButtonText;
        public Color ImageColor = new Color(0, 1, 1, 0.5f);
        public Color TextColor = Color.black;
        public Vector2 TargetPos;
        public float TargetScale;
        public float TargetAlpha;
        private float _currentAlpha;
        public BaseOrder Order;

        private void Awake()
        {
            ButtonImage = GetComponent<Image>();
            ButtonText = GetComponentInChildren<Text>();
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
            transform.position = TargetPos * 0.1f + (Vector2)transform.position * 0.9f;
            transform.localScale = Vector3.one * TargetScale * 0.1f + 
                                   transform.localScale * 0.9f;
            _currentAlpha = TargetAlpha * 0.1f + _currentAlpha * 0.9f;
            SetAlpha(_currentAlpha);
        }
    }
}