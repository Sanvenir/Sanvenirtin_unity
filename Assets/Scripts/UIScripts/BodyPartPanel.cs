using System.Collections.Generic;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;
using UnityEngine;

namespace UIScripts
{
    public class BodyPartPanel : MonoBehaviour
    {
        public BodyPartButton BodyPartButton;
        [HideInInspector] public List<BodyPartButton> BodyPartButtonInstances;
        public Character Player;

        public void GenerateBodyPart(BodyPart bodyPart)
        {
            var instance = Instantiate(BodyPartButton, transform);
            instance.BodyPart = bodyPart;
            BodyPartButtonInstances.Add(instance);
        }

        private void OnEnable()
        {
            foreach (var part in Player.BodyParts.Values) GenerateBodyPart(part);
        }
    }
}