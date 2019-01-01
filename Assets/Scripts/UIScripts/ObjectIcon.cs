using ObjectScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.CharacterController;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts
{
    public class ObjectIcon: MonoBehaviour, IPointerDownHandler
    {
        public Text ObjectName;
        public Image ObjectImage;
        
        [HideInInspector]
        public BaseObject BaseObject;

        public BodyPartSelectMenu BodyPartSelectMenu;

        private PlayerController _playerController;

        private PickupAction _pickupAction;

        private void Start()
        {
            _playerController = SceneManager.Instance.PlayerController;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            _pickupAction = new PickupAction(_playerController.Character)
            {
                TargetObject = BaseObject
            };

            _pickupAction.RefreshFetchParts();
            BodyPartSelectMenu.StartUp(transform.position, _pickupAction.FetchParts);
        }

        private void LateUpdate()
        {
            if (!Input.GetMouseButtonUp(1)) return;
            var result = BodyPartSelectMenu.EndUp();
            if (result == null) return;

            _pickupAction.FetchPart = result;
            _playerController.NextAction = _pickupAction;
            
            if (SceneManager.Instance.ObjectListMenu.GetComponentsInChildren<ObjectIcon>().Length == 1)
            {
                SceneManager.Instance.ObjectListMenu.EndUp();
            }
            Destroy(gameObject);
        }
    }
}