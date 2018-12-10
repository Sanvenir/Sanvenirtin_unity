using ObjectScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts
{
    public class SceneControlButton: Button
    {
        public ActionMenu ActionMenu;

        protected override void Awake()
        {
            base.Awake();
            ActionMenu = GetComponent<ActionMenu>();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            var mousePos = SceneManager.Instance.MainCamera.ScreenToWorldPoint(eventData.position);
            ActionMenu.StartUp(mousePos);
            
            var hit = Physics2D.OverlapPoint(mousePos);
            if (hit == null) return;
            if (hit.GetComponent<Substance>())
            {
                SceneManager.Instance.Print(hit.name);
            }
        }
    }
}