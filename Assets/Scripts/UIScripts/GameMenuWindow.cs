using UnityEngine;
using UnityEngine.EventSystems;

namespace UIScripts
{
    public abstract class GameMenuWindow: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _isPointerIn;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerIn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerIn = false;
        }

        private void Update()
        {
            if (_isPointerIn) return;
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                EndUp();
            }
        }

        public abstract void EndUp();
    }
}