using ObjectScripts.CharSubstance;
using UtilScripts;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    /// <inheritdoc />
    /// <summary>
    ///     Order to control player
    /// </summary>
    public abstract class BaseOrder : INamed
    {
        public static PlayerController Controller
        {
            get { return SceneManager.Instance.PlayerController; }
        }

        public static Character Player
        {
            get { return SceneManager.Instance.PlayerObject; }
        }

        public abstract string GetTextName();

        /// <summary>
        ///     Called when player is on turn; And usually need to call the AddAction function of player
        /// </summary>
        /// <returns>The next order for player controller, null means no order or invalid order</returns>
        public virtual BaseOrder DoOrder()
        {
            Controller.SetAction(null);
            return null;
        }

        /// <summary>
        ///     Used in order selection list
        /// </summary>
        /// <returns>If not to show it in the list, return false</returns>
        public abstract bool CheckAndSet();
    }
}