using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace GameJam
{
    public class Server : MonoBehaviour
    {  
        public int UserCapacity = 100;
        public int RequiredPower = 100;
        public bool IsOnline = false;

        public List<CableEndPoint> PowerConnections = new List<CableEndPoint>();
        public List<CableEndPoint> DataConnections = new List<CableEndPoint>();

        public Temperature Temperature = new Temperature();

        public GameObject FireEffects;
        public GameObject IndicatorLight;

        public int ServeServer(int users)
        {
            if (Temperature.Overheated())
            {
                FireEffects.SetActive(true);
                SetIndicatorColor(Color.red);
                IsOnline = false;
                return 0;
            }
            else
            {
                FireEffects.SetActive(false);
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
                SetIndicatorColor(Color.red);
                IsOnline = false;
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
                SetIndicatorColor(Color.red);
                IsOnline = false;
                return 0;
            }

            if (users > UserCapacity)
            {
                users = UserCapacity;
            }

            float capacityPercentage = (float)users / UserCapacity;
            Temperature.UpdateHeat(capacityPercentage);
            if (capacityPercentage >= Temperature.TemperatureThreshold)
            {
                SetIndicatorColor(Color.yellow);
            }
            else
            {
                SetIndicatorColor(Color.green);
            }

            IsOnline = true;
            return users;
        }

        public void RandomAttack()
        {
            // TODO support more kinds of attacks, make this better
            if (Random.Range(0, 1) == 0)
            {
                var connections = PowerConnections.Where(item => item.IsConnected()).ToList();
                if (connections.Count > 0)
                {
                    int index = Random.Range(0, connections.Count);
                    connections[index].BreakConnection();
                }
            }
            else
            {
                var connections = DataConnections.Where(item => item.IsConnected()).ToList();
                if (connections.Count > 0)
                {
                    int index = Random.Range(0, connections.Count);
                    connections[index].BreakConnection();
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

        private void SetIndicatorColor(Color color)
        {
            IndicatorLight.GetComponentInChildren<Light>().color = color;
        }
    }
}