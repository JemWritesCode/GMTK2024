using System;
using TMPro;
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
        public Light IndicatorLight;

        public bool TryBlockAttack()
        {
            // TODO Balance
            // Chance when down = 20%
            // Chance when low integrity = 10%
            // Chance when up = 5%

            float attackChance = 0.05f;

            if (Temperature.Overheated())
            {
                FireEffects.SetActive(true);
                SetIndicatorColor(Color.red);

                attackChance = 0.2f;
            }
            else
            {
                FireEffects.SetActive(false);
                if (Integrity < IntegrityThreshold * MaximumIntegrity)
                {
                    attackChance = 0.1f;
                }
            }

            if (UnityEngine.Random.Range(0, 1f) > attackChance)
            {
                UpdateIndicatorColor();
                return true;
            }

            // Hit heat and integrity working to block attacks
            Temperature.UpdateHeat();
            Integrity = Math.Clamp(Integrity - IntegrityTicks, 0, MaximumIntegrity);
            GameObject.Find("FirewallPercentInWorldText").GetComponent<TextMeshPro>().text = Integrity.ToString() + "%";
            Debug.Log("jem here hallo");
            UpdateIndicatorColor();
            return false;
        }

        public void FireWallInteract(GameObject interactAgent)
        {
            if (HandManager.Instance.HoldingItem())
            {
                HandManager.Instance.UseItem(this);
                UpdateIndicatorColor();
            }
        }

        public void FireWallPasswordInteract(GameObject interactAgent)
        {
            if (!HandManager.Instance.HoldingItem())
            {
                Integrity = MaximumIntegrity;
                var dispenser = GetComponent<Dispenser>();
                if (dispenser != null)
                {
                    dispenser.DispenserInteract(interactAgent);
                }
                UpdateIndicatorColor();
            }
        }

        private void UpdateIndicatorColor()
        {
            if (Integrity == 0 || Temperature.Overheated())
            {
                SetIndicatorColor(Color.red);
            }
            else if (Integrity <= IntegrityThreshold * MaximumIntegrity)
            {
                SetIndicatorColor(Color.yellow);
            }
            else if (Temperature.HeatReachingCritical())
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
            IndicatorLight.color = color;
        }
    }
}