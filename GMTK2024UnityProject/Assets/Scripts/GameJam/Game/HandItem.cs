using UnityEngine;

namespace GameJam
{
    public class HandItem : MonoBehaviour
    {
        public bool Consumable = false;
        public bool CanPutOutFire = false;
        public int HeatReduction = 10;

        private bool pickedUp = false;
        private bool usable = true;

        void Update()
        {
            if (pickedUp)
            {
                var transform = InteractManager.Instance.InteractAgent.transform;
                this.transform.position = transform.forward + transform.position;
            }
        }

        public void UseItem(Temperature temperature)
        {
            if (!usable)
            {
                return;
            }

            if (CanPutOutFire)
            {
                temperature.ResetHeat();
            }
            else if (!temperature.Overheated())
            {
                temperature.LowerHeat(HeatReduction);
            }
            else
            {
                return;
            }

            if (Consumable)
            {
                this.GetComponent<Interactable>().CanInteract = false;
                usable = false;
            }
        }

        public void Pickup()
        {
            if (HandManager.Instance.TryPickup(this))
            {
                this.GetComponent<Interactable>().CanInteract = false;
                pickedUp = true;
            }
        }

        public void Drop()
        {
            if (IsUsable())
            {
                this.GetComponent<Interactable>().CanInteract = true;
            }

            this.GetComponent<Rigidbody>().velocity = InteractManager.Instance.InteractAgent.transform.forward + Vector3.up * 3f;
            pickedUp = false;
        }

        public bool IsUsable()
        {
            return usable;
        }
    }
}