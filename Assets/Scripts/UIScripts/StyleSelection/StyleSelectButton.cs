using System.Collections.Generic;
using ObjectScripts.CharacterController;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UtilScripts;

namespace UIScripts.StyleSelection
{
    public abstract class StyleSelectButton : MonoBehaviour, IPointerDownHandler
    {
        private bool _isActivate;
        public SelectionMenu SelectionMenu;
        public Text Text;

        protected static PlayerController PlayerController
        {
            get { return SceneManager.Instance.PlayerController; }
        }

        protected static Character Player
        {
            get { return PlayerController.Character; }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            _isActivate = true;
            SelectionMenu.StartUp(transform.position, GetStyleSelections());
        }

        private void Start()
        {
            Text = GetComponentInChildren<Text>();
        }

        protected abstract IEnumerable<INamed> GetStyleSelections();

        private void LateUpdate()
        {
            if (!_isActivate || Input.GetMouseButton(1)) return;
            _isActivate = false;
            Selected(SelectionMenu.EndUp());
        }

        protected abstract void Selected(INamed selection);
    }
}