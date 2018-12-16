using System;
using ObjectScripts;
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
            HealthBar.maxValue = Player.GetMaxHealth();
            SanityBar.maxValue = Player.GetMaxSanity();
            EndureBar.maxValue = Player.GetMaxEndure();
            HungerBar.maxValue = Player.GetMaxHunger();

            HealthBar.value = Player.Health;
            SanityBar.value = Player.Sanity;
            EndureBar.value = Player.Endure;
            HungerBar.value = Player.Hunger;
            HealthValue.text = string.Format("Health\t {0:F0}", Player.Health);
            SanityValue.text = string.Format("Sanity\t {0:F0}", Player.Sanity);
            EndureValue.text = string.Format("Endure\t {0:F0}", Player.Endure);
            HungerValue.text = string.Format("Hunger\t {0:F0}", Player.Hunger);
            MaradyValue.text = string.Format("Marady\t {0:F0}", Player.Marady);
        }
    }
}