using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UtilScripts;

namespace UIScripts
{
    public class PanelButton : MonoBehaviour, IPointerDownHandler
    {
        public delegate void CalledFunction(object selection);

        private CalledFunction _calledFunction;

        private bool _isClicked;

        private IEnumerable<INamed> _selectionList;
        public SelectionMenu SelectionMenu;
        public Text Text;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (_selectionList == null)
            {
                _calledFunction(null);
                return;
            }

            SelectionMenu.StartUp(transform.position, _selectionList);
            _isClicked = true;
        }

        public void Initialize(string text, CalledFunction function, IEnumerable<INamed> selection = null)
        {
            Text.text = text;
            _selectionList = selection;
            _calledFunction = function;
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButton(0) || !_isClicked) return;
            var selection = SelectionMenu.EndUp();
            _calledFunction(selection);
            _isClicked = false;
        }
    }
}