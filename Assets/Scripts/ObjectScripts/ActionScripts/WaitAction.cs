namespace ObjectScripts.ActionScripts
{
    public class WaitAction: BasicAction
    {
        public override int DoAction()
        {
            return Self.GetAttackTime();
        }
    }
}