namespace ObjectScripts.ActionScripts
{
    public class WaitAction: BasicAction
    {
        public override void DoAction(bool check = true)
        {
            Self.ActivateTime += Self.GetReactTime();
        }

        public override bool CheckAction()
        {
            return true;
        }

        public WaitAction(Character self) : base(self)
        {
        }
    }
}