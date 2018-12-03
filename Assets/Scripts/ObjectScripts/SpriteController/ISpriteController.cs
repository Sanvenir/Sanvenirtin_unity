using UtilScripts;

namespace ObjectScripts.SpriteController
{
    public interface ISpriteController
    {
        void SetDirection(Direction direction);
        bool IsMoving();
        void StartMoving();
        void StopMoving();
    }
}