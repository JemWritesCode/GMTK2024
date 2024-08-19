using UnityEngine;

using YoloBox;

namespace GameJam
{
    public sealed class HandManager : SingletonManager<HandManager>
    {
        [field: SerializeField]
        public CableStartPoint CurrentCable { get; set; }

        [field: SerializeField]
        public HandItem Item { get; set; }

        public bool HoldingCable()
        {
            return CurrentCable != null;
        }

        public bool HoldingItem()
        {
            return Item != null;
        }

        public bool ProcessInteractable()
        {
            Interactable interactable = InteractManager.Instance.ClosestInteractable;

            if (!interactable)
            {
                if (HoldingCable())
                {
                    CurrentCable.CancelConnection();
                    return true;
                }

                if (HoldingItem())
                {
                    Item.Drop();
                    Instance.Item = null;
                    return true;
                }
            }

            return false;
        }

        public bool TryPickup(HandItem item)
        {
            if (!HoldingCable() && !HoldingItem() && !item.Used)
            {
                Item = item;
                return true;
            }

            return false;
        }

        public bool TryPickup(CableStartPoint cable)
        {
            if (!HoldingCable() && !HoldingItem())
            {
                //CurrentCable = cable;
                return true;
            }

            return false;
        }

        public void UseItem(Server server)
        {
            if (server && Item && Item.UseItem(server))
            {
                ConsumeItem();
            }
        }

        public void UseItem(FireWall firewall)
        {
            if (firewall && Item && Item.UseItem(firewall))
            {
                ConsumeItem();
            }
        }

        private void ConsumeItem()
        {
            if (Item.Consumable)
            {
                Item.GetComponent<Interactable>().CanInteract = false;
                Item.Used = true;
                Item.Drop();
                Item = null;
            }
        }
    }
}
