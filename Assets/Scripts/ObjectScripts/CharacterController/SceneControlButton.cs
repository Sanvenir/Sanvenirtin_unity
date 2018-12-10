using AreaScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace ObjectScripts.CharacterController
{
    public class SceneControlButton: Button
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            var mousePos = SceneManager.Instance.MainCamera.ScreenToWorldPoint(eventData.position);

            SceneManager.Instance.ActionMenu.transform.position = 
                SceneManager.Instance.NormalizeWorldPos(mousePos);
            SceneManager.Instance.ActionMenu.gameObject.SetActive(true);
            
            var hit = Physics2D.OverlapPoint(mousePos);
            if (hit == null) return;
            if (hit.GetComponent<Substance>())
            {
                SceneManager.Instance.Print(hit.name);
            }
        }
    }
}