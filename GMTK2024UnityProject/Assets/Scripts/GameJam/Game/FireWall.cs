using System;
using TMPro;
using UnityEngine;

namespace GameJam
{
    public class FireWall : MonoBehaviour
    {
        public float MaximumIntegrity = 100;
        public float Integrity = 100;
        public float IntegrityThreshold = 0.1f;
        public float IntegrityTicks = 0.2f;

        public Temperature Temperature = new Temperature();
        public GameObject FireEffects;
        public GameObject SmokeEffects;
        public Light IndicatorLight;
        public TextMeshPro IntegrityText;

        [Header("Attack chances")]
        public float AttackChanceUp = 0.01f;
        public float AttackChanceVulnerable = 0.02f;
        public float AttackChanceDown = 0.05f;

        public void UpdateDisplay()
        {
            if (IntegrityText != null)
            {
                IntegrityText.text = ((int)Integrity).ToString() + "%";
            }
        }

        public bool TryBlockAttack()
        {
            Integrity = Math.Clamp(Integrity - IntegrityTicks, 0, MaximumIntegrity);
            Temperature.PlayEffects(FireEffects, SmokeEffects);
            float attackChance = AttackChanceUp;

            if (Temperature.Overheated())
            {
                SetIndicatorColor(Color.red);

                attackChance = AttackChanceDown;
            }
            else
            {
                if (Integrity < IntegrityThreshold * MaximumIntegrity)
                {
                    attackChance = AttackChanceVulnerable;
                }
            }

            if (UnityEngine.Random.Range(0, 1f) > attackChance)
            {
                UpdateDisplay();
                UpdateIndicatorColor();
                return true;
            }

            // Hit heat working to block attacks
            Temperature.UpdateHeat();
            UpdateDisplay();
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
            if (!Temperature.Overheated() && !HandManager.Instance.HoldingItem())
            {
                Integrity = MaximumIntegrity;
                var dispenser = GetComponent<Dispenser>();
                if (dispenser != null)
                {
                    dispenser.DispenserInteract(interactAgent);
                }
                UpdateDisplay();
                UpdateIndicatorColor();
            }
        }

        private void UpdateIndicatorColor()
        {
            if (Integrity == 0 || Temperature.Overheated())
            {
                SetIndicatorColor(Color.red);
            }
            else if (Temperature.HeatReachingCritical())
            {
                SetIndicatorColor(Color.yellow);
            }
            else if (Integrity <= IntegrityThreshold * MaximumIntegrity)
            {
                SetIndicatorColor(new Color(0.616f, 0, 1));
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