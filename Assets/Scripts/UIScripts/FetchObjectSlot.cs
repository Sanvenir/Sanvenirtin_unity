using ObjectScripts;
using ObjectScripts.BodyPartScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class FetchObjectSlot: MonoBehaviour
    {
        public Image FetchObjectImage;

        [HideInInspector] public BaseObject FetchObject = null;
        [HideInInspector] public BodyPart BodyPart = null;

        private void Update()
        {
            if (FetchObject == null)
            {
                FetchObjectImage.enabled = false;
            }
            gameObject.SetActive(BodyPart.Available);
        }

        public void AddObject(BaseObject fetchObject)
        {
            FetchObject = fetchObject;
            FetchObjectImage.sprite = fetchObject.SpriteRenderer.sprite;
            FetchObjectImage.enabled = true;
        }
    }
}