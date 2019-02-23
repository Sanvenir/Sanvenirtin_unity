using ObjectScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.StyleScripts.ActStyleScripts;
using UtilScripts;

namespace UIScripts
{
    public class PartSelectPanel : GameMenuWindow
    {
        public PanelButton PartSelectButtonPrefab;
        
        public void StartUp(Direction targetDirection, ActActionSkill actionSkill)
        {
            EndUp();
            Substance target = null;
            foreach (var attackPlace in actionSkill.GetAttackPlaces(targetDirection, Player))
            {
                target = BaseObject.GetObject<Substance>(attackPlace, SceneManager.Instance.AttackLayer);
                if (target != null) break;
            }

            if (target == null)
            {
                return;
            }
            
            gameObject.SetActive(true);
            
            foreach (var bodyPart in target.GetBodyParts(actionSkill.TargetPartPos))
            {
                var instance = Instantiate(PartSelectButtonPrefab, transform);
                var part = bodyPart;
                instance.Initialize(
                    part.TextName, 
                    delegate
                    {
                        PlayerController.SetAction(
                            new ActStabAction(Player, actionSkill, targetDirection, part, target));
                        EndUp();
                    });
            }
        }
        
        public override void EndUp()
        {
            foreach (var button in GetComponentsInChildren<PanelButton>())
                Destroy(button.gameObject);
            gameObject.SetActive(false);
        }
    }
}