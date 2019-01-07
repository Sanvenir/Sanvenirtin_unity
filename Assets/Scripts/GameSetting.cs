using System.Collections.Generic;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.RaceScripts;
using UnityEngine;
using UtilScripts.Text;

public class GameSetting: MonoBehaviour
{
        public List<BodyPartList> ComponentList;
        public List<BasicRace> RaceList = new List<BasicRace>
        {
                {
                        new BasicRace()
                }
        };
        public static GameSetting Instance = null;

        private void Awake()
        {
                if (Instance == null)
                {
                        Instance = this;
                }

                else if (Instance == this)
                {
                        Destroy(gameObject);
                }

                DontDestroyOnLoad(gameObject);
        }
}