using System.Collections.Generic;
using ObjectScripts.ActionScripts;
using UnityEngine;

namespace UIScripts
{
    public class ActionMenu: MonoBehaviour
    {
        public GameObject ButtonPrefab;
        
        public GameObject[] ButtonInstances;
        public string[] ButtonText;
        public ActionMenu[] NextMenus;
        
        public int CenterIndex;

        public ActionMenu PrevMenu;

        public Vector2 PrevMousePos;
        public Vector2 CenterPos;

        public void StartUp(Vector2 pos)
        {
            gameObject.SetActive(true);
            CenterPos = pos;
            PrevMousePos = Input.mousePosition;
            CenterIndex = 0;
        }
        
        private void Update()
        {
            
        }
    }
}