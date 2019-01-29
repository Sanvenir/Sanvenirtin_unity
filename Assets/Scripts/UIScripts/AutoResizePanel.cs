using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts
{
    public class AutoResizePanel: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int DelayTime = 10;
        public int ExpandWidth = 150;
        
        private Vector2 _originSize;
        private Vector2 _expandSize;
        private RectTransform _rectTransform;
        private bool _activate = false;
        private int _timeCount = 0;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originSize = _rectTransform.sizeDelta;
            _expandSize = new Vector2(ExpandWidth, Screen.height);
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