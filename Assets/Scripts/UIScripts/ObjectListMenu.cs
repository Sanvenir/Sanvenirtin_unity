using ObjectScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class ObjectListMenu: MonoBehaviour
    {
        public GridLayoutGroup Content;
        public ObjectIcon IconPrefab;

        public void StartUp(Collider2D[] colliderList)
        {
            EndUp();
            gameObject.SetActive(true);
            foreach (var objectCollider in colliderList)
            {
                if (objectCollider == null) return;

                var baseObject = objectCollider.GetComponent<BaseObject>();
                if (baseObject == null) continue;
                
                var instance = Instantiate(IconPrefab, Content.transform);
                instance.ObjectImage.sprite = baseObject.SpriteRenderer.sprite;
                instance.ObjectName.text = baseObject.TextName;
                instance.BaseObject = baseObject;
            }
        }

        public void EndUp()
        {
            foreach (var child in Content.GetComponentsInChildren<ObjectIcon>())
            {
                Destroy(child.gameObject);
            }

            gameObject.SetActive(false);
        }
    }
}