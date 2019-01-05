using ObjectScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.CharacterController;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts
{
    public class ObjectIcon : MonoBehaviour, IPointerDownHandler
    {
        public Text ObjectName;
        public Image ObjectImage;

        [HideInInspector] public BaseObject BaseObject;

        public BodyPartSelectMenu BodyPartSelectMenu;

        private PlayerController _playerController;

        private void Start()
        {
            _playerController = SceneManager.Instance.PlayerController;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            BodyPartSelectMenu.StartUp(transform.position, _playerController.Character.GetFreeFetchParts());
        }

        private void LateUpdate()
        {
            if (!Input.GetMouseButtonUp(1)) return;
            var result = BodyPartSelectMenu.EndUp();
            if (result == null) return;

            _playerController.SetAction(new PickupAction(_playerController.Character, BaseObject, result));

            if (SceneManager.Instance.ObjectListMenu.GetComponentsInChildren<ObjectIcon>().Length == 1)
            {
                SceneManager.Instance.ObjectListMenu.EndUp();
            }

            Destroy(gameObject);
        }
    }
}