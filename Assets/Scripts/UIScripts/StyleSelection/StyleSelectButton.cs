using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIScripts.StyleSelection
{
    public abstract class StyleSelectButton: MonoBehaviour, IPointerDownHandler
    {
        private bool _isActivate = false;
        public SelectionMenu SelectionMenu;

        protected abstract IEnumerable<object> GetStyleSelections();
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            _isActivate = true;
            SelectionMenu.StartUp(transform.position, GetStyleSelections());
        }

        private void LateUpdate()
        {
            if (!_isActivate || Input.GetMouseButton(1)) return;
            _isActivate = false;
            SelectionMenu.EndUp();
        }
    }
}