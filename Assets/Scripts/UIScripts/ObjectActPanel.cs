using System;
using ObjectScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharacterController;
using ObjectScripts.CharSubstance;
using ObjectScripts.ItemScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UtilScripts.Text;

namespace UIScripts
{
    public class ObjectActPanel : GameMenuWindow
    {
        public Button ActButtonPrefab;
        public Image ItemImage;
        public Text ItemInfoTest;
        public VerticalLayoutGroup ActButtonLayout;

        [HideInInspector] public BaseObject BaseObject;

        private Button _buttonInstance;

        private PlayerController _playerController;

        private Character Player
        {
            get { return _playerController.Character; }
        }

        public void StartUp(BaseObject baseObject, BodyPart bodyPart = null)
        {
            EndUp();
            gameObject.SetActive(true);
            _playerController = SceneManager.Instance.PlayerController;
            BaseObject = baseObject;

            var character = BaseObject as Character;
            if (character != null && !character.Dead) return;

            if (bodyPart != null || Player.CheckInteractRange(baseObject.WorldPos))
            {
                // Cases that object can be interact;
                if (baseObject is IConsumableItem)
                {
                    AddButton(GameText.Instance.ConsumeAct, delegate
                    {
                        _playerController.SetAction(new ConsumeAction(
                            _playerController.Character,
                            (IConsumableItem) baseObject));
                    });
                }

                if (baseObject is ComplexObject)
                {
                    AddButton(GameText.Instance.SplitAct, delegate
                    {
                        _playerController.SetAction(new SplitAction(
                            _playerController.Character,
                            (ComplexObject) baseObject));
                    });
                }
            }

            if (bodyPart != null)
            {
                // Cases that object is at body part;
                AddButton(GameText.Instance.DropAct, delegate
                {
                    _playerController.SetAction(new DropFetchAction(_playerController.Character, bodyPart));
                    EndUp();
                });
            }
            else
            {
                // Cases that object is not at body part
                AddButton(GameText.Instance.PickupOrder, delegate
                {
                    // TODO: Add fetch action here
                });
                _buttonInstance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
                
            }
        }

        private void AddButton(string text, UnityAction listener)
        {
            _buttonInstance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
            _buttonInstance.GetComponentInChildren<Text>().text = text;
            _buttonInstance.onClick.AddListener(listener);
        }

        public override void EndUp()
        {
            foreach (var button in ActButtonLayout.GetComponentsInChildren<Button>()) Destroy(button.gameObject);
            gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if (BaseObject == null)
            {
                EndUp();
                return;
            }

            ItemImage.sprite = BaseObject.SpriteRenderer.sprite;
            ItemInfoTest.text = BaseObject.Info + "\n" + BaseObject.GetConditionDescribe();
        }
    }
}