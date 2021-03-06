using System.Collections.Generic;
using ObjectScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharacterController;
using UnityEngine;

namespace UIScripts
{
    public class FetchObjectList : MonoBehaviour
    {
        private Dictionary<BodyPart, BaseObject> _fetchDictionary;
        private PlayerController _playerController;

        [HideInInspector] public Dictionary<BodyPart, FetchObjectSlot> FetchSlots;

        public FetchObjectSlot SlotPrefab;

        private void Start()
        {
            _playerController = SceneManager.Instance.PlayerController;
            _fetchDictionary = _playerController.Character.FetchDictionary;
            FetchSlots = new Dictionary<BodyPart, FetchObjectSlot>();
            foreach (var part in _fetchDictionary.Keys)
            {
                if (!part.Available) continue;
                var instance = Instantiate(SlotPrefab, transform);
                instance.BodyPart = part;
                FetchSlots.Add(part, instance);
            }
        }

        private void Update()
        {
            foreach (var part in _fetchDictionary.Keys)
            {
                if (_fetchDictionary[part] == null)
                {
                    FetchSlots[part].RemoveObject();
                    continue;
                }

                FetchSlots[part].AddObject(_fetchDictionary[part]);
            }
        }
    }
}