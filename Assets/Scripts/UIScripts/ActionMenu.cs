using System.Collections.Generic;
using ObjectScripts.CharacterController;
using ObjectScripts.CharacterController.PlayerOrder;
using UnityEngine;
using UtilScripts;

namespace UIScripts
{
    public class ActionMenu: SelectionMenu
    {
        private static PlayerController PlayerController
        {
            get { return SceneManager.Instance.PlayerController; }
        }
        
        public List<BaseOrder> OrderList = new List<BaseOrder>
        {
            new WalkToOrder(),
            new WaitOrder(),
            new LookAtOrder(),
            new PickupOrder(),
            new RestOrder(),
//            new AttackDirectionOrder(),
        };

        private IEnumerable<INamed> GetAvailableOrders()
        {
            foreach (var order in OrderList)
            {
                if (order.CheckAndSet()) yield return order;
            }
            if (PlayerController.ActSetterList == null) yield break;
            foreach (var setter in PlayerController.ActSetterList)
            {
                var order = new BasicActOrder(setter);
                if (order.CheckAndSet()) yield return order;
            }
        }

        public void StartUp(Vector2 pos)
        {
            base.StartUp(pos, GetAvailableOrders());
        }
    }
}