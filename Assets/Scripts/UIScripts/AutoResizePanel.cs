using UnityEngine;
using UnityEngine.EventSystems;

namespace UIScripts
{
    public class AutoResizePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _activate;
        private Vector2 _expandSize;

        private Vector2 _originSize;
        private RectTransform _rectTransform;
        private int _timeCount;
        public int DelayTime = 10;
        public int ExpandWidth = 150;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _activate = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _timeCount = 0;
            _activate = false;
        }

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originSize = _rectTransform.sizeDelta;
            _expandSize = new Vector2(ExpandWidth, Screen.height);
        }

        private void Update()
        {
            if (_activate && ++_timeCount > DelayTime)
            {
                _timeCount = DelayTime;
                _rectTransform.sizeDelta =
                    _rectTransform.sizeDelta * 0.9f +
                    _expandSize * 0.1f;
            }
            else
            {
                _rectTransform.sizeDelta =
                    _rectTransform.sizeDelta * 0.9f +
                    _originSize * 0.1f;
            }
        }
    }
}