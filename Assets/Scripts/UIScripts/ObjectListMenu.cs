using System;
using ObjectScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts
{
    public class ObjectListMenu: GameMenuWindow
    {
        public GridLayoutGroup Content;
        public ObjectIcon IconPrefab;

        public void StartUp(Collider2D[] colliderList, bool isInteractive)
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
                instance.IsAvailable = isInteractive;
            }
        }

        public override void EndUp()
        {
            foreach (var child in Content.GetComponentsInChildren<ObjectIcon>())
            {
                Destroy(child.gameObject);
            }

            gameObject.SetActive(false);
        }
    }
}