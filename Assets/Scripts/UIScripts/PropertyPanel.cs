using System;
using ObjectScripts;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class PropertyPanel: MonoBehaviour
    {
        public Slider HealthBar;
        public Slider SanityBar;
        public Slider EndureBar;
        public Slider HungerBar;
        public Slider MaradyBar;
        public Character Player;

        public Text HealthValue;
        public Text SanityValue;
        public Text EndureValue;
        public Text HungerValue;
        public Text MaradyValue;

        private void Update()
        {
            HealthBar.maxValue = Player.Properties.GetMaxHealth();
            SanityBar.maxValue = Player.Properties.GetMaxSanity();
            EndureBar.maxValue = Player.Properties.GetMaxEndure();
            HungerBar.maxValue = Player.Properties.GetMaxHunger();

            HealthBar.value = Player.Properties.Health;
            SanityBar.value = Player.Properties.Sanity;
            EndureBar.value = Player.Properties.Endure;
            HungerBar.value = Player.Properties.Hunger;
            HealthValue.text = string.Format("Health\t {0:F2}", Player.Properties.Health);
            SanityValue.text = string.Format("Sanity\t {0:F2}", Player.Properties.Sanity);
            EndureValue.text = string.Format("Endure\t {0:F2}", Player.Properties.Endure);
            HungerValue.text = string.Format("Hunger\t {0:F2}", Player.Properties.Hunger);
            MaradyValue.text = string.Format("Marady\t {0:F2}", Player.Properties.Marady);
        }
    }
}