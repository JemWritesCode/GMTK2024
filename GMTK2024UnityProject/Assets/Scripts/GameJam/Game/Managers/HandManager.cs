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

            if (!interactable)
            {
                if (HoldingCable())
                {
                    Debug.Log($"Nothing to interact with, drop cable.");
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
            if (!HoldingCable() && !HoldingItem() && item.IsUsable())
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
                CurrentCable = cable;
                return true;
            }

            return false;
        }

        public void UseItem(Temperature temperature)
        {
            if (Item.UseItem(temperature))
            {
                ConsumeItem();
                // TODO: possibly check fire effects here too 
                //FireEffects.SetActive(false);
            }
        }

        private void ConsumeItem()
        {
            if (Item.Consumable)
            {
                // TODO: change appearance of can
                Item.Drop();
                Item = null;
            }
        }
    }
}
