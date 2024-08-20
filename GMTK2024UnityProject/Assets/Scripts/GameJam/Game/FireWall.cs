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
        public Light IndicatorLight;

        [Header("Attack chances")]
        public float AttackChanceUp = 0.01f;
        public float AttackChanceVulnerable = 0.02f;
        public float AttackChanceDown = 0.05f;

        public bool TryBlockAttack()
        {
            float attackChance = AttackChanceUp;

            if (Temperature.Overheated())
            {
                FireEffects.SetActive(true);
                SetIndicatorColor(Color.red);

                attackChance = AttackChanceDown;
            }
            else
            {
                FireEffects.SetActive(false);
                if (Integrity < IntegrityThreshold * MaximumIntegrity)
                {
                    attackChance = AttackChanceVulnerable;
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