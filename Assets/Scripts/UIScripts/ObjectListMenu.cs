using System;
using System.Collections.Generic;
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

        public void StartUp(List<BaseObject> objectList, bool isInteractive)
        {
            EndUp();
            gameObject.SetActive(true);
            foreach (var baseObject in objectList)
            {
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