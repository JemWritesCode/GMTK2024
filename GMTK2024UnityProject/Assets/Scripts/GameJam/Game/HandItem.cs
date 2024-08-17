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

        public void UseItem(Server server)
        {
            if (!usable)
            {
                return;
            }

            if (CanPutOutFire)
            {
                server.ResetHeat();
            }
            else
            {
                server.LowerHeat(HeatReduction);
            }

            if (Consumable)
            {
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
            this.GetComponent<Interactable>().CanInteract = true;
            this.GetComponent<Rigidbody>().velocity = InteractManager.Instance.InteractAgent.transform.forward + Vector3.up * 3f;
            pickedUp = false;
        }

        public void Disable()
        {
            usable = false;
        }
    }
}