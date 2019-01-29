using System.Collections.Generic;
using ObjectScripts.CharacterController.PlayerOrder;
using UnityEngine;
using UtilScripts;

namespace UIScripts
{
    public class ActionMenu: SelectionMenu
    {
        
        public List<BaseOrder> OrderList = new List<BaseOrder>
        {
            new WalkToOrder(),
            new WaitOrder(),
            new LookAtOrder(),
            new PickupOrder(),
            new RestOrder(),
            new AttackDirectionOrder(),
        };

        private IEnumerable<INamed> GetAvailableOrders()
        {
            foreach (var order in OrderList)
            {
                if (order.CheckAndSet()) yield return order;
            }
        }

        public void StartUp(Vector2 pos)
        {
            base.StartUp(pos, GetAvailableOrders());
        }
    }
}