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
            Debug.Log($"Holding item, interacting.");
            Interactable interactable = InteractManager.Instance.ClosestInteractable;

            if (interactable)
            {
                interactable.Interact(InteractManager.Instance.InteractAgent);
            }
            else
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
            if (!HoldingCable() && !HoldingItem())
            {
                Item = item;
                return true;
            }

            return false;
        }

        public void UseItem(Server server)
        {
            Item.UseItem(server);

            if (Item.Consumable)
            {
                // TODO: change appearance of can
                Item.Disable();
                Item = null;
            }
        }
    }
}
