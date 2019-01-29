using System;
using System.Collections.Generic;
using ObjectScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.CharacterController;
using ObjectScripts.ItemScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UtilScripts;

namespace UIScripts
{
    public class ObjectActButton: MonoBehaviour, IPointerDownHandler
    {
        public Text Text;
        public SelectionMenu SelectionMenu;

        private IEnumerable<INamed> _selectionList;

        private CalledFunction _calledFunction;
        
        public delegate void CalledFunction(object selection);
        
        private bool _isClicked;

        public void Initialize(string text, CalledFunction function, IEnumerable<INamed> selection = null)
        {
            Text.text = text;
            _selectionList = selection;
            _calledFunction = function;
        }

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

        private void LateUpdate()
        {
            if (Input.GetMouseButton(0) || !_isClicked) return;
            var selection = SelectionMenu.EndUp();
            _calledFunction(selection);
            _isClicked = false;
        }
    }
}