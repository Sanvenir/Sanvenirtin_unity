using ObjectScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using BodyPart = ObjectScripts.BodyPartScripts.BodyPart;

namespace UIScripts
{
    public class BodyPartButton: MonoBehaviour
    {
        public Slider HitPointBar;
        public Slider CutPointBar;
        public Text Text;
        [HideInInspector]
        public BodyPart BodyPart;

        private void Start()
        {
            Text.text = BodyPart.GetTextName();
        }

        private void Update()
        {
            if (!BodyPart.Available)
            {
                CutPointBar.value = 0;
                return;
            };
            HitPointBar.maxValue = BodyPart.HitPoint.MaxValue;
            HitPointBar.value = BodyPart.HitPoint.Value;
            CutPointBar.maxValue = BodyPart.CutPoint.MaxValue;
            CutPointBar.value = BodyPart.CutPoint.Value;
        }
    }
}