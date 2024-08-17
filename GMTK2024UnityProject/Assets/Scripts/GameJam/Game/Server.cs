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

        public List<CableEndPoint> PowerConnections = new List<CableEndPoint>();
        public List<CableEndPoint> DataConnections = new List<CableEndPoint>();

        public int ServeServer(int users)
        {
            if (Heat >= HeatLimit)
            {
                // FIRE FIRE
                return 0;
            }

            bool connected = false;
            foreach (CableEndPoint p in DataConnections)
            {
                if (p.IsConnected())
                {
                    connected = true;
                    break;
                }
            }

            if (!connected)
            {
                return 0;
            }

            foreach (CableEndPoint p in PowerConnections)
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
            if (Heat < 0)
            {
                Heat = 0;
            }
        }

        public void ResetHeat()
        {
            Heat = 0;
        }

        public void RandomAttack()
        {
            if (Random.Range(0, 1) == 0)
            {
                int index = Random.Range(0, PowerConnections.Count);
                if (PowerConnections[index].IsConnected())
                {
                    PowerConnections[index].BreakConnection();
                }
            }
            else
            {
                int index = Random.Range(0, DataConnections.Count);
                if (DataConnections[index].IsConnected())
                {
                    DataConnections[index].BreakConnection();
                }
            }
        }

        public void ServerInteract(GameObject interactAgent)
        {
            Debug.Log("Interact With Server...");
            if (HandManager.Instance.HoldingItem())
            {
                HandManager.Instance.UseItem(this);
            }
        }
    }
}