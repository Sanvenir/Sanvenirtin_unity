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
        public PanelButton ActButtonPrefab;
        public Image ItemImage;
        public Text ItemInfoTest;
        public VerticalLayoutGroup ActButtonLayout;

        [HideInInspector] public BaseObject BaseObject;

        [HideInInspector] public SelectionMenu BodyPartSelectMenu;

        private PanelButton _buttonInstance;

        private static PlayerController PlayerController
        {
            get { return SceneManager.Instance.PlayerController; }
        }

        private static Character Player
        {
            get { return PlayerController.Character; }
        }

        public void StartUp(BaseObject baseObject, BodyPart bodyPart = null)
        {
            EndUp();
            gameObject.SetActive(true);
            BaseObject = baseObject;

            var character = BaseObject as Character;
            if (character != null && !character.Dead) return;

            if (bodyPart != null || Player.CheckInteractRange(baseObject.WorldPos))
            {
                // Cases that object can be interact;
                if (baseObject is IConsumableItem)
                {
                    var instance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
                    instance.Initialize(
                        GameText.Instance.ConsumeAct,
                        delegate
                        {
                            PlayerController.SetAction(new ConsumeAction(
                                PlayerController.Character,
                                (IConsumableItem) baseObject));
                        });
                }

                var complexObject = baseObject as ComplexObject;
                if (complexObject != null)
                {
                    var instance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
                    instance.Initialize(
                        GameText.Instance.SplitAct,
                        delegate(object selection)
                        {
                            PlayerController.SetAction(new SplitAction(
                                PlayerController.Character,
                                complexObject, selection as BodyPart));
                        }, complexObject.GetAvailableBodyParts());
                }
            }

            if (bodyPart != null)
            {
                // Cases that object is at body part;
                var instance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
                instance.Initialize(
                    GameText.Instance.DropAct,
                    delegate
                    {
                        PlayerController.SetAction(new DropFetchAction(PlayerController.Character, bodyPart));
                        EndUp();
                    });
            }
            else if (Player.CheckInteractRange(baseObject.WorldPos))
            {
                // Cases that object is not at body part but in the interact range
                var instance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
                instance.Initialize(
                    GameText.Instance.PickupOrder,
                    delegate(object selection)
                    {
                        if (selection == null) return;
                        PlayerController.SetAction(new PickupAction(PlayerController.Character, BaseObject,
                            selection as BodyPart));
                        EndUp();
                    }, Player.GetFreeFetchParts());
            }
        }

        public override void EndUp()
        {
            foreach (var button in ActButtonLayout.GetComponentsInChildren<PanelButton>())
                Destroy(button.gameObject);
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