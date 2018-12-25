using ObjectScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.CharacterController;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts
{
    public class ObjectIcon: MonoBehaviour, IPointerClickHandler
    {
        public Text ObjectName;
        public Image ObjectImage;
        
        [HideInInspector]
        public BaseObject BaseObject;

        private PlayerController _playerController;

        private void Start()
        {
            _playerController = SceneManager.Instance.PlayerController;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            var action = new PickupAction(_playerController.Character)
            {
                TargetObject = BaseObject
            };

            if (!action.CheckAction()) return;
            _playerController.NextAction = action;
            Destroy(gameObject);
        }
    }
}