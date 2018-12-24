using ObjectScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class ObjectListMenu: MonoBehaviour
    {
        public Collider2D[] ColliderList;
        public GridLayoutGroup Content;
        public ObjectIcon IconPrefab;

        private void OnEnable()
        {
            foreach (var objectCollider in ColliderList)
            {
                if (objectCollider == null) return;

                var baseObject = objectCollider.GetComponent<BaseObject>();
                if (baseObject == null) continue;
                
                var instance = Instantiate(IconPrefab, Content.transform);
                instance.ObjectImage.sprite = baseObject.SpriteRenderer.sprite;
                instance.ObjectName.text = baseObject.Name;
            }
        }

        private void OnDisable()
        {
            foreach (var child in Content.GetComponentsInChildren<ObjectIcon>())
            {
                Destroy(child.gameObject);
            }
        }
    }
}