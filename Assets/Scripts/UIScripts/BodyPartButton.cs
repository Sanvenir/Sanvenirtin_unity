using ObjectScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using BodyPart = ObjectScripts.BodyPartScripts.BodyPart;

namespace UIScripts
{
    public class BodyPartButton: MonoBehaviour
    {
        [HideInInspector]
        public Slider DurabilityBar;
        [HideInInspector]
        public Text Text;
        [HideInInspector]
        public BodyPart BodyPart;

        private void Awake()
        {
            DurabilityBar = GetComponent<Slider>();
            Text = GetComponentInChildren<Text>();
        }

        private void Start()
        {
            Text.text = BodyPart.Name;
        }

        private void Update()
        {
            DurabilityBar.maxValue = BodyPart.Durability.MaxValue;
            DurabilityBar.value = BodyPart.Durability.Value;
        }
    }
}