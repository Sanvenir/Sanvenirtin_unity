using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class PropertyPanel : MonoBehaviour
    {
        public Slider EndureBar;
        public Text EndureValue;
        public Slider HealthBar;

        public Text HealthValue;
        public Slider HungerBar;
        public Text HungerValue;
        public Slider MaradyBar;
        public Text MaradyValue;
        public Character Player;
        public Slider SanityBar;
        public Text SanityValue;

        private void Update()
        {
            HealthBar.maxValue = Player.Properties.GetMaxHealth(0);
            SanityBar.maxValue = Player.Properties.GetMaxSanity(0);
            EndureBar.maxValue = Player.Properties.GetMaxEndure(0);
            HungerBar.maxValue = Player.Properties.GetMaxHunger(0);

            HealthBar.value = Player.Health;
            SanityBar.value = Player.Sanity;
            EndureBar.value = Player.Endure;
            HungerBar.value = Player.Hunger;
            HealthValue.text = string.Format("Health\t {0:F2}", Player.Health);
            SanityValue.text = string.Format("Sanity\t {0:F2}", Player.Sanity);
            EndureValue.text = string.Format("Endure\t {0:F2}", Player.Endure);
            HungerValue.text = string.Format("Hunger\t {0:F2}", Player.Hunger);
            MaradyValue.text = string.Format("Marady\t {0:F2}", Player.Marady);
        }
    }
}