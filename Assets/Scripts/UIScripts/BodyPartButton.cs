using ObjectScripts.BodyPartScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class BodyPartButton : MonoBehaviour
    {
        [HideInInspector] public BodyPart BodyPart;

        public Slider CutPointBar;
        public Slider HitPointBar;
        public Text Text;

        private void Start()
        {
            Text.text = BodyPart.GetTextName();
        }

        private void Update()
        {
            if (!BodyPart.Available)
            {
                CutPointBar.value = 0;
                return;
            }

            ;
            HitPointBar.maxValue = BodyPart.HitPoint.MaxValue;
            HitPointBar.value = BodyPart.HitPoint.Value;
            CutPointBar.maxValue = BodyPart.CutPoint.MaxValue;
            CutPointBar.value = BodyPart.CutPoint.Value;
        }
    }
}