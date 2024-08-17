using System;
using UnityEngine;

namespace GameJam
{
    public class FireWall : MonoBehaviour
    {
        public int MaximumIntegrity = 100;
        public int Integrity = 100;
        public int IntegrityThreshold = 20;

        public Temperature Temperature = new Temperature();
        public GameObject FireEffects;

        public bool TryBlockAttack(int reputation)
        {
            if (Temperature.Overheated())
            {
                FireEffects.SetActive(true);
                if (reputation < 0 || UnityEngine.Random.Range(0, 10) < 1)
                {
                    return false;
                }
                else
                {
                    // Only small chance to be attacked if good reputations
                    return true;
                }
            }
            else
            {
                FireEffects.SetActive(false);
            }

            if (reputation > 0)
            {
                // No bad attacks
                return true;
            }

            // Hit heat and integrity working to block attacks
            Temperature.UpdateHeat();

            ChangeIntegrity(reputation % 100);

            if (UnityEngine.Random.Range(0, Integrity) > IntegrityThreshold)
            {
                return true;
            }

            return false;
        }

        public void ChangeIntegrity(int integrity)
        {
            Integrity += integrity;
            Math.Clamp(Integrity, 0, MaximumIntegrity);
        }

        public void FireWallInteract(GameObject interactAgent)
        {
            Debug.Log("Interact With FireWall...");
            if (HandManager.Instance.HoldingItem())
            {
                HandManager.Instance.UseItem(Temperature);
            }
            else
            {
                ChangeIntegrity(MaximumIntegrity);
            }
        }
    }
}