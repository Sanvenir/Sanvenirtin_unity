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
        }
    }
}