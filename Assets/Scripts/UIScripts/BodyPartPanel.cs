using System.Collections.Generic;
using ObjectScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharacterController;
using UnityEngine;

namespace UIScripts
{
    public class BodyPartPanel: MonoBehaviour
    {
        public Character Player;
        public BodyPartButton BodyPartButton;
        [HideInInspector] public List<BodyPartButton> BodyPartButtonInstances;

        public void GenerateBodyPart(BodyPart bodyPart)
        {
            var instance = Instantiate(BodyPartButton, transform);
            instance.BodyPart = bodyPart;
            BodyPartButtonInstances.Add(instance);
        }

        private void OnEnable()
        {
            foreach (var part in Player.GetAllBodyParts())
            {
                GenerateBodyPart(part);
            }
        }
    }
}