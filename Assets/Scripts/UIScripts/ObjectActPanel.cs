using ObjectScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharacterController;
using ObjectScripts.CharSubstance;
using ObjectScripts.ItemScripts;
using UnityEngine;
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
                if (baseObject is IConsumableItem)
                {
                    _buttonInstance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
                    _buttonInstance.GetComponentInChildren<Text>().text = GameText.Instance.ConsumeAct;
                    _buttonInstance.onClick.AddListener(delegate
                    {
                        _playerController.SetAction(new ConsumeAction(
                            _playerController.Character,
                            (IConsumableItem) baseObject));
                    });
                }

                if (baseObject is ComplexObject)
                {
                    _buttonInstance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
                    _buttonInstance.GetComponentInChildren<Text>().text = GameText.Instance.SplitAct;
                    _buttonInstance.onClick.AddListener(delegate
                    {
                        _playerController.SetAction(new SplitAction(
                            _playerController.Character,
                            (ComplexObject) baseObject));
                    });
                }
            }

            if (bodyPart != null)
            {
                _buttonInstance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
                _buttonInstance.GetComponentInChildren<Text>().text = GameText.Instance.DropAct;
                _buttonInstance.onClick.AddListener(delegate
                {
                    _playerController.SetAction(new DropFetchAction(_playerController.Character,
                        baseObject, bodyPart));
                    EndUp();
                });
            }
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