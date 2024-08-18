using System;
using UnityEngine;

namespace GameJam
{
    public class FireWall : MonoBehaviour
    {
        public int MaximumIntegrity = 100;
        public int Integrity = 100;
        public float IntegrityThreshold = 0.1f;
        public int IntegrityTicks = 10;

        public Temperature Temperature = new Temperature();
        public GameObject FireEffects;
        public GameObject IndicatorLight;

        public bool TryBlockAttack(int reputation)
        {
            if (Temperature.Overheated())
            {
                FireEffects.SetActive(true);
                SetIndicatorColor(Color.red);

                if (reputation < 0 || UnityEngine.Random.Range(0, 10) < 1)
                {
                    Integrity = Math.Clamp(Integrity - IntegrityTicks, 0, MaximumIntegrity);
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

            // Random chance for attack to succeed, more so if bad reputation
            float threshold = (reputation < 0) ? IntegrityThreshold : 0;
            if (UnityEngine.Random.Range(0, Integrity) > threshold * MaximumIntegrity)
            {
                UpdateIndicatorColor();
                return true;
            }

            // Hit heat and integrity working to block attacks
            Temperature.UpdateHeat();
            Integrity = Math.Clamp(Integrity - IntegrityTicks, 0, MaximumIntegrity);

            UpdateIndicatorColor();

            return false;
        }

        public void FireWallInteract(GameObject interactAgent)
        {
            Debug.Log("Interact With FireWall...");
            if (HandManager.Instance.HoldingItem())
            {
                HandManager.Instance.UseItem(Temperature);
                UpdateIndicatorColor();
            }
        }

        public void FireWallPasswordInteract(GameObject interactAgent)
        {
            Debug.Log("Interact With FireWall panel...");
            if (!HandManager.Instance.HoldingItem())
            {
                Integrity = MaximumIntegrity;
                UpdateIndicatorColor();
            }
        }

        private void UpdateIndicatorColor()
        {
            if (Integrity == 0)
            {
                SetIndicatorColor(Color.red);
            }
            else if (Integrity <= IntegrityThreshold * MaximumIntegrity)
            {
                SetIndicatorColor(Color.yellow);
            }
            else
            {
                SetIndicatorColor(Color.green);
            }
        }

        private void SetIndicatorColor(Color color)
        {
            IndicatorLight.GetComponentInChildren<Light>().color = color;
        }
    }
}