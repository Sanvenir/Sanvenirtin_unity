using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public abstract class BaseOrder: INamed
    {
        public static PlayerController Controller
        {
            get { return SceneManager.Instance.PlayerController; }
        }
        public virtual BaseOrder DoOrder()
        {
            // rtype: The next order for player controller, null means no order or invalid order
            Controller.SetAction(null);
            return null;
        }

        public abstract string GetTextName();
        public abstract bool IsAvailable();
    }
}