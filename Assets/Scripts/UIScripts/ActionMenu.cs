using System.Collections;
using System.Collections.Generic;
using ObjectScripts.ActionScripts;
using ObjectScripts.CharacterController.PlayerOrder;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UtilScripts;

namespace UIScripts
{
    public class ActionMenu: MonoBehaviour
    {
        
        // Setting
        [Range(0.01f, 0.1f)]
        public float ChangeAccuracy = 0.1f;
        
        public ActionButton ButtonPrefab;
        public Canvas MenuCanvas;
        
        public List<ActionButton> ButtonInstances;
        
        public int CenterIndex;
        public BaseOrder CurrentOrder;

        public Vector2 PrevMousePos;
        public Vector2 CenterPos;

        private int _step;
        private int _index;

        public void StartUp(Vector2 pos, List<BaseOrder> orderList)
        {
            enabled = true;
            CenterPos = pos;
            PrevMousePos = Input.mousePosition;
            CenterIndex = 0;
            ButtonInstances = new List<ActionButton>();
            foreach (var order in orderList)
            {
                var instance = Instantiate(ButtonPrefab, MenuCanvas.transform);
                instance.SetText(order.Name);
                instance.transform.position = pos;
                instance.Order = order;
                ButtonInstances.Add(instance);
            }

            CurrentOrder = ButtonInstances[CenterIndex].Order;

        }
        
        public BaseOrder EndUp()
        {
            enabled = false;
            foreach (var button in ButtonInstances)
            {
                Destroy(button.gameObject);
            }

            return CurrentOrder;
        }
        
        private void Update()
        {
            for (_index = 0; _index != ButtonInstances.Count; ++_index)
            {
                ButtonInstances[_index].TargetPos = CenterPos + Vector2.up * (CenterIndex - _index) * 1.0f;
                ButtonInstances[_index].TargetAlpha = 1.0f;
                ButtonInstances[_index].TargetScale = _index == CenterIndex ? 1.0f : 0.8f;
            }

            CenterIndex = Utils.FloatToInt((PrevMousePos.y - Input.mousePosition.y) * ChangeAccuracy);
            if (CenterIndex >= ButtonInstances.Count)
            {
                CenterIndex = ButtonInstances.Count - 1;
            }
            if (CenterIndex < -1)
            {
                CenterIndex = -1;
            }
            CurrentOrder = CenterIndex >= 0 ? ButtonInstances[CenterIndex].Order : null;
        }
    }
}