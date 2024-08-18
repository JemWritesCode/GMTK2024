using UnityEngine;

namespace GameJam
{
    public class HandItem : MonoBehaviour
    {
        public bool Consumable = false;
        public bool Used = false;
        public bool CanPutOutFire = false;
        public int HeatReduction = 10;
        public bool CanResetVirus = false;
        public GameObject UseEffects;

        private bool pickedUp = false;

        void Update()
        {
            if (pickedUp)
            {
                var transform = InteractManager.Instance.InteractAgent.transform;
                this.transform.position = transform.forward + transform.position;
            }
        }

        public bool UseItem(Server server)
        {
            if (Used)
            {
                return false;
            }

            if (CanResetVirus)
            {
                if (server.HasVirus)
                {
                    server.SetVirus(false);
                }
                else
                {
                    return false;
                }
            }
            else if (!ModifyTemperature(server.Temperature))
            {
                return false;
            }

            PlayEffects();

            return true;
        }

        public bool UseItem(FireWall firewall)
        {
            if (Used || !ModifyTemperature(firewall.Temperature))
            {
                return false;
            }

            PlayEffects();

            return true;
        }

        private bool ModifyTemperature(Temperature temperature)
        {
            if (temperature == null)
            {
                return false;
            }

            if (temperature.Overheated())
            {
                if (CanPutOutFire)
                {
                    temperature.ResetHeat();
                    return true;
                }
            }
            else if (!CanPutOutFire && HeatReduction > 0)
            {
                temperature.LowerHeat(HeatReduction);
                return true;
            }

            return false;
        }

        private void PlayEffects()
        {
            UseEffects.SetActive(true);
            foreach (var effect in UseEffects.GetComponentsInChildren<ParticleSystem>())
            {
                effect.Play();
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
            if (!Used)
            {
                this.GetComponent<Interactable>().CanInteract = true;
            }

            this.GetComponent<Rigidbody>().velocity = InteractManager.Instance.InteractAgent.transform.forward + Vector3.up * 3f;
            pickedUp = false;
        }
    }
}