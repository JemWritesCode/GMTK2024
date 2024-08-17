using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Server : MonoBehaviour
    {
        public int UserCapacity = 100;
        public int Heat = 0;
        public int HeatLimit = 100;
        public int RequiredPower = 100;

        public List<CablePoint> PowerConnections = new List<CablePoint>();

        public int ServeServer(int users)
        {
            foreach (CablePoint p in PowerConnections)
            {
                if (!p.IsConnected())
                {
                    continue;
                }

                if (p.Connection.TryGetComponent<PowerBox>(out PowerBox powerBox))
                {
                    if (powerBox.GetPower() < RequiredPower)
                    {
                        return 0;
                    }
                }

                // IDEAS: can check for different cable connections than power boxes here, upgrades!
            }

            if (users > UserCapacity)
            {
                users = UserCapacity;
            }

            float capacityPercentage = (float)users / UserCapacity;
            UpdateHeat(capacityPercentage);

            return users;
        }

        private void UpdateHeat(float capacityPercentage)
        {
            if (capacityPercentage >= 0.8)
            {
                Heat++;
            }

            if (Heat > HeatLimit)
            {
                // TODO FIRE FIRE FIRE
            }
        }

        public void LowerHeat(int heat)
        {
            Heat -= heat;
        }
    }
}