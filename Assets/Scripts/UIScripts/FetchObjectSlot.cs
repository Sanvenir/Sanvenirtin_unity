using ObjectScripts;
using ObjectScripts.BodyPartScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts
{
    public class FetchObjectSlot : MonoBehaviour, IPointerClickHandler
    {
        [HideInInspector] public BodyPart BodyPart;

        [HideInInspector] public BaseObject FetchObject;
        public Image FetchObjectImage;


        public void OnPointerClick(PointerEventData eventData)
        {
            if (FetchObject == null) return;
            SceneManager.Instance.ObjectActPanel.StartUp(FetchObject, BodyPart);
        }

        private void Update()
        {
            if (FetchObject == null) FetchObjectImage.enabled = false;
            gameObject.SetActive(BodyPart.Available);
        }

        public void AddObject(BaseObject fetchObject)
        {
            FetchObject = fetchObject;
            FetchObjectImage.sprite = fetchObject.SpriteRenderer.sprite;
            FetchObjectImage.enabled = true;
        }

        public void RemoveObject()
        {
            FetchObject = null;
        }
    }
}