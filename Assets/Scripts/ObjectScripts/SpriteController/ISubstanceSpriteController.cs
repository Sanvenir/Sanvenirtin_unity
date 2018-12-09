using UtilScripts;

namespace ObjectScripts.SpriteController
{
    public interface ISubstanceSpriteController
    {
        void SetDirection(Direction direction);
        bool IsMoving();
        void StartMoving();
        void StopMoving();
        void SetDisable(bool disabled);
    }
}