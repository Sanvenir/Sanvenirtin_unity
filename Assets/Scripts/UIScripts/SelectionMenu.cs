using System.Collections.Generic;
using UnityEngine;
using UtilScripts;

namespace UIScripts
{
    public class SelectionMenu : MonoBehaviour
    {
        private int _index;


        private float _mouseY;

        private int _step;

        [HideInInspector] public List<SelectionButton> ButtonInstances = new List<SelectionButton>();

        public SelectionButton ButtonPrefab;

        [Range(0.01f, 10f)] public float ButtonSpace = 1.0f;

        [HideInInspector] public int CenterIndex;

        [HideInInspector] public Vector2 CenterPos;

        // Setting
        [Range(0.01f, 10f)] public float ChangeAccuracy = 0.1f;

        [HideInInspector] public INamed CurrentSelect;

        [HideInInspector] public List<INamed> SelectionList;

        public void StartUp(Vector2 pos, IEnumerable<INamed> selectionList)
        {
            SceneManager.Instance.PlayerController.CurrentOrder = null;
            CenterPos = pos;
            SelectionList = new List<INamed>();
            foreach (var selection in selectionList)
            {
                var instance = Instantiate(ButtonPrefab, transform);
                var named = selection;
                if (named != null) instance.SetText(named.GetTextName());
                instance.transform.position = CenterPos;
                ButtonInstances.Add(instance);
                SelectionList.Add(selection);
            }

            if (SelectionList.Count == 0) return;

            enabled = true;
            _mouseY = 0.5f;
            CenterIndex = 0;
            CurrentSelect = SelectionList[CenterIndex];
        }

        public INamed EndUp()
        {
            if (!enabled) return default(INamed);
            enabled = false;
            foreach (var button in ButtonInstances) Destroy(button.gameObject);
            ButtonInstances = new List<SelectionButton>();
            return CurrentSelect;
        }

        private void Update()
        {
            for (_index = 0; _index != ButtonInstances.Count; ++_index)
            {
                ButtonInstances[_index].TargetPos = CenterPos + Vector2.up * (CenterIndex - _index) * ButtonSpace;
                ButtonInstances[_index].TargetAlpha = 1.0f;
                ButtonInstances[_index].TargetScale = _index == CenterIndex ? 1.0f : 0.8f;
            }

            _mouseY -= Input.GetAxis("Mouse Y") * ChangeAccuracy;
            CenterIndex = Utils.FloatToInt(_mouseY);
            if (CenterIndex >= ButtonInstances.Count)
            {
                CenterIndex = ButtonInstances.Count - 1;
                _mouseY = Mathf.Min(_mouseY, CenterIndex + 1);
            }
            else if (CenterIndex < -1)
            {
                CenterIndex = -1;
                _mouseY = Mathf.Max(_mouseY, -1);
            }

            CurrentSelect = CenterIndex >= 0 ? SelectionList[CenterIndex] : default(INamed);
        }
    }
}