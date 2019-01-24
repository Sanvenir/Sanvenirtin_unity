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
    public class SelectionMenu: MonoBehaviour
    {
        
        // Setting
        [Range(0.01f, 10f)]
        public float ChangeAccuracy = 0.1f;
        
        public SelectionButton ButtonPrefab;
        
        [HideInInspector]
        public List<object> SelectionList;
        
        [HideInInspector]
        public List<SelectionButton> ButtonInstances = new List<SelectionButton>();
        
        [HideInInspector]
        public int CenterIndex;
        
        [HideInInspector]
        public object CurrentSelect;
        
        
        private float _mouseY;
        
        [HideInInspector]
        public Vector2 CenterPos;

        private int _step;
        private int _index;

        public void StartUp(Vector2 pos, IEnumerable<object> selectionList)
        {
            SceneManager.Instance.PlayerController.CurrentOrder = null;
            CenterPos = pos;
            SelectionList = new List<object>();
            foreach (var selection in selectionList)
            {
                var instance = Instantiate(ButtonPrefab, transform);
                var named = selection as INamed;
                if (named != null)
                {
                    instance.SetText(named.GetTextName());
                } 
                instance.transform.position = CenterPos;
                ButtonInstances.Add(instance);
                SelectionList.Add(selection);
            }

            if (SelectionList.Count == 0) return;

            enabled = true;
            _mouseY = 0f;
            CenterIndex = 0;
            CurrentSelect = SelectionList[CenterIndex];
        }
        
        public object EndUp()
        {
            if (!enabled) return default(object);
            enabled = false;
            foreach (var button in ButtonInstances)
            {
                Destroy(button.gameObject);
            }
            ButtonInstances = new List<SelectionButton>();
            return CurrentSelect;
        }
        
        private void Update()
        {
            for (_index = 0; _index != ButtonInstances.Count; ++_index)
            {
                ButtonInstances[_index].TargetPos = CenterPos + Vector2.up * (CenterIndex - _index);
                ButtonInstances[_index].TargetAlpha = 1.0f;
                ButtonInstances[_index].TargetScale = _index == CenterIndex ? 1.0f : 0.8f;
            }

            _mouseY -= Input.GetAxis("Mouse Y");
            CenterIndex = Utils.FloatToInt(_mouseY * ChangeAccuracy);
            if (CenterIndex >= ButtonInstances.Count)
            {
                CenterIndex = ButtonInstances.Count - 1;
                _mouseY = Mathf.Min(_mouseY, CenterIndex / ChangeAccuracy + 1);
            }
            else if (CenterIndex < -1)
            {
                CenterIndex = -1;
                _mouseY = Mathf.Max(_mouseY, -1 / ChangeAccuracy - 1);
            }

            CurrentSelect = CenterIndex >= 0 ? SelectionList[CenterIndex] : default(object);
        }
    }
}