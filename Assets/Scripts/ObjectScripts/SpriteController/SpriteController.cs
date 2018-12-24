using UnityEngine;
using UtilScripts;

namespace ObjectScripts.SpriteController
{
    public abstract class SpriteController: MonoBehaviour
    {
        public abstract void SetDirection(Direction direction);
        public abstract bool IsMoving();
        public abstract void StartMoving();
        public abstract void StopMoving();
        public abstract void SetDisable(bool disabled);
    }
}