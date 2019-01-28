using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts
{
    public class StylePanel: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int DelayTime = 10;
        public int ExpandWidth = 400;
        
        private Vector2 _originSize;
        private RectTransform _rectTransform;
        private Image _image;
        private bool _activate = false;
        private int _timeCount = 0;

        private void Start()
        {
            _image = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
            _originSize = _rectTransform.sizeDelta;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _activate = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _timeCount = 0;
            _activate = false;
        }

        private void Update()
        {
            if (_activate && ++_timeCount > DelayTime)
            {
                _timeCount = DelayTime;
                _rectTransform.sizeDelta = 
                    _rectTransform.sizeDelta * 0.9f +
                    (_originSize + Vector2.right * ExpandWidth) * 0.1f;
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