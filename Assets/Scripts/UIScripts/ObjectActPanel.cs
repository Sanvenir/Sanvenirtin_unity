using System.Collections.Generic;
using ObjectScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.BasicItem;
using ObjectScripts.CharacterController;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIScripts
{
    public class ObjectActPanel: MonoBehaviour
    {
        public Button ActButtonPrefab;
        public Image ItemImage;
        public Text ItemInfoTest;
        public VerticalLayoutGroup ActButtonLayout;
        
        [HideInInspector]
        public BaseObject BaseObject;

        private Button _buttonInstance;
        
        private PlayerController _playerController;

        public void StartUp(BaseObject baseObject)
        {
            gameObject.SetActive(true);
            _playerController = SceneManager.Instance.PlayerController;
            BaseObject = baseObject;
            ItemImage.sprite = baseObject.SpriteRenderer.sprite;
            ItemInfoTest.text = baseObject.Info.ToString();
            
            if (baseObject is IConsumeItem)
            {
                _buttonInstance = Instantiate(ActButtonPrefab, ActButtonLayout.transform);
                _buttonInstance.GetComponentInChildren<Text>().text = "Consume";
                _buttonInstance.onClick.AddListener(delegate
                {
                    _playerController.NextAction = 
                        new ConsumeAction(_playerController.Character, (IConsumeItem)baseObject);
                });
            }
        }

        public void EndUp()
        {
            foreach (var button in ActButtonLayout.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }

            gameObject.SetActive(false);
        }

        private void Update()
        {
            if(BaseObject == null) EndUp();
        }
    }
}