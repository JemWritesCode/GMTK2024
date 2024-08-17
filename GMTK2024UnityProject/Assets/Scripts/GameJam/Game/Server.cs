using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Server : MonoBehaviour
    {
        public int UserCapacity = 100;
        public int RequiredPower = 100;

        public List<CableEndPoint> PowerConnections = new List<CableEndPoint>();
        public List<CableEndPoint> DataConnections = new List<CableEndPoint>();

        public Temperature Temperature = new Temperature();

        public int ServeServer(int users)
        {
            if (Temperature.Overheated())
            {
                // TODO: Play/update fire animation
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

            int totalPower = 0;

            foreach (CableEndPoint p in PowerConnections)
            {
                if (!p.IsConnected())
                {
                    continue;
                }
                
                PowerBox powerBox = p.Connection.GetComponentInParent<PowerBox>();
                if (powerBox != null)
                {
                    totalPower += powerBox.GetPower();
                }

                // IDEAS: can check for different cable connections than power boxes here, upgrades!
            }

            if (totalPower < RequiredPower)
            {
                return 0;
            }

            if (users > UserCapacity)
            {
                users = UserCapacity;
            }

            float capacityPercentage = (float)users / UserCapacity;
            Temperature.UpdateHeat(capacityPercentage);

            return users;
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
                HandManager.Instance.UseItem(Temperature);
            }
        }
    }
}