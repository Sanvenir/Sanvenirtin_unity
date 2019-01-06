using System.Collections.Generic;
using ObjectScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.BasicItem;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharacterController;
using ObjectScripts.CharSubstance;
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

        public void StartUp(BaseObject baseObject, BodyPart bodyPart = null)
        {
            EndUp();
            gameObject.SetActive(true);
            _playerController = SceneManager.Instance.PlayerController;
            BaseObject = baseObject;
            ItemImage.sprite = baseObject.SpriteRenderer.sprite;
            ItemInfoTest.text = baseObject.Info.ToString();

            if (baseObject is IConsumeItem)
            {
                _buttonInstance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
                _buttonInstance.GetComponentInChildren<Text>().text = GameText.Instance.ConsumeAct;
                _buttonInstance.onClick.AddListener(delegate
                {
                    _playerController.SetAction(new ConsumeAction(
                        _playerController.Character,
                        (IConsumeItem) baseObject));
                });
            }

            if (bodyPart != null)
            {
                _buttonInstance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
                _buttonInstance.GetComponentInChildren<Text>().text = GameText.Instance.DropAct;
                _buttonInstance.onClick.AddListener(delegate
                {
                    _playerController.SetAction(new DropAction(_playerController.Character,
                        baseObject, bodyPart));
                    EndUp();
                });
            }
        }

        public override void EndUp()
        {
            foreach (var button in ActButtonLayout.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }

            gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if (BaseObject == null) EndUp();
        }
    }
}