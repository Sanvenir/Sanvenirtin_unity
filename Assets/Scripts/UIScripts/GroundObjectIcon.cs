using System.Collections.Generic;
using ObjectScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharacterController;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts
{
    public class GroundObjectIcon : MonoBehaviour, IPointerDownHandler
    {
        public Text ObjectName;
        public Image ObjectImage;

        [HideInInspector] public BaseObject BaseObject;
        [HideInInspector] public bool IsAvailable;

        public SelectionMenu BodyPartSelectMenu;

        private PlayerController _playerController;

        private void Start()
        {
            _playerController = SceneManager.Instance.PlayerController;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            if (!IsAvailable)
                SceneManager.Instance.ObjectActPanel.StartUp(BaseObject);
            else
                BodyPartSelectMenu.StartUp(transform.position,
                    _playerController.Character.GetFreeFetchParts());
        }

        private void LateUpdate()
        {
            if (!Input.GetMouseButtonUp(1) || !IsAvailable) return;

            var result = BodyPartSelectMenu.EndUp();
            if (result == null) return;

            _playerController.SetAction(new PickupAction(_playerController.Character, BaseObject, result as BodyPart));

            if (SceneManager.Instance.GroundObjectListMenu.GetComponentsInChildren<GroundObjectIcon>().Length == 1)
                SceneManager.Instance.GroundObjectListMenu.EndUp();

            Destroy(gameObject);
        }
    }
}