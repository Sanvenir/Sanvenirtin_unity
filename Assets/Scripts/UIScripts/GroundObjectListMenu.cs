using System;
using System.Collections.Generic;
using ObjectScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts
{
    public class GroundObjectListMenu: GameMenuWindow
    {
        public GridLayoutGroup Content;
        public GroundObjectIcon IconPrefab;

        public void StartUp(List<BaseObject> objectList, bool isInteractive)
        {
            EndUp();
            if (objectList.Count == 1)
            {
                var baseObject = objectList[0];
                if (baseObject == null) return;
                SceneManager.Instance.ObjectActPanel.StartUp(baseObject);
                return;
            }
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
            foreach (var child in Content.GetComponentsInChildren<GroundObjectIcon>())
            {
                Destroy(child.gameObject);
            }

            gameObject.SetActive(false);
        }
    }
}