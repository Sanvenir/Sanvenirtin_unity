using ObjectScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using BodyPart = ObjectScripts.BodyPartScripts.BodyPart;

namespace UIScripts
{
    public class BodyPartButton: MonoBehaviour
    {
        public Slider DurabilityBar;
        public Text Text;
        [HideInInspector]
        public BodyPart BodyPart;

        private void Start()
        {
            Text.text = BodyPart.Name;
        }

        private void Update()
        {
            DurabilityBar.maxValue = BodyPart.HitPoint.MaxValue;
            DurabilityBar.value = BodyPart.HitPoint.Value;
        }
    }
}