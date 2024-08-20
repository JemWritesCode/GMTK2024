using UnityEngine;

namespace GameJam
{
    [System.Serializable]
    public sealed class Temperature
    {
        [SerializeField]
        public float Heat = 0;
        [SerializeField]
        public int HeatLimit = 100;
        [SerializeField]
        public float HeatTicks = 1f;

        [SerializeField]
        public float TemperatureThreshold = 0.8f;

        public float HeatPercent()
        {
            return Heat / (float)HeatLimit;
        }

        public bool HeatReachingCritical()
        {
            return (Heat / (float)HeatLimit) >= TemperatureThreshold;
        }

        public bool Overheated()
        {
            return Heat >= HeatLimit;
        }

        public void UpdateHeat()
        {
            Heat = Mathf.Clamp(Heat + HeatTicks, 0, HeatLimit);
        }

        public void LowerHeat(int heat)
        {
            Heat = Mathf.Clamp(Heat - heat, 0, HeatLimit);
        }

        public void ResetHeat()
        {
            Heat = 0;
        }

        public void PlayEffects(GameObject fireEffects, GameObject smokeEffects)
        {
            if (fireEffects != null && smokeEffects != null)
            {
                var fireParticles = fireEffects.GetComponentsInChildren<ParticleSystem>();
                var smokeParticles = smokeEffects.GetComponentsInChildren<ParticleSystem>();

                bool hot = HeatReachingCritical();
                bool overheated = Overheated();

                foreach (var effect in smokeParticles)
                {
                    if (hot)
                    {
                        effect.Play();
                    }
                    else
                    {
                        effect.Stop();
                    }
                }

                foreach (var effect in fireParticles)
                {
                    if (overheated)
                    {
                        effect.Play();
                    }
                    else
                    {
                        effect.Stop();
                    }
                }
            }
        }
    }
}
