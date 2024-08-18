using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace GameJam
{
    public class Server : MonoBehaviour
    {  
        public int UserCapacity = 100;
        public int CurrentUsers = 0;
        public float UserThreshold = 0.8f;
        public int RequiredPower = 100;
        public bool Online = false;
        public bool HasVirus = false;

        public List<CableEndPoint> PowerConnections = new List<CableEndPoint>();
        public List<CableEndPoint> DataConnections = new List<CableEndPoint>();

        public Temperature Temperature = new Temperature();

        public GameObject FireEffects;

        public Light IndicatorLight;
        public float LightBlinkInterval = 0.5f;
        private float lightBlinkTimer;

        private void Start()
        {
            UserCapacity = Random.Range(0, 999);
            RequiredPower = Random.Range(0, 999);
            Temperature.Heat = Random.Range(0, Temperature.HeatLimit);
        }

        private void Update()
        {
            if (IndicatorLight && HasVirus)
            {
                lightBlinkTimer += Time.deltaTime;
                if (lightBlinkTimer > LightBlinkInterval)
                {
                    IndicatorLight.enabled = !IndicatorLight.enabled;
                    lightBlinkTimer = 0;
                }
            }
        }

        public int ServeServer(int users)
        {
            if (Temperature.Overheated())
            {
                FireEffects.SetActive(true);
            }
            else
            {
                FireEffects.SetActive(false);
            }

            if (!DataConnected() || !PowerConnected() || Temperature.Overheated() || HasVirus)
            {
                SetOnline(false);
                CurrentUsers = 0;
            }
            else
            {
                SetOnline(true);

                if (users > UserCapacity)
                {
                    users = UserCapacity;
                }

                float capacityPercentage = (float)users / UserCapacity;
                Temperature.UpdateHeat(capacityPercentage);
            }

            UpdateIndicatorColor();
            return users;
        }

        private bool DataConnected()
        {
            bool connected = false;

            foreach (CableEndPoint p in DataConnections)
            {
                if (p.IsConnected())
                {
                    connected = true;
                    break;
                }
            }

            return connected;
        }

        private bool PowerConnected()
        {
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
            }

            if (totalPower < RequiredPower)
            {
                return false;
            }

            return true;
        }

        public void SetOnline(bool online)
        {
            Online = online;
        }

        public bool IsOnline()
        {
            return Online && !HasVirus;
        }

        public void SetVirus(bool virus)
        {
            HasVirus = virus;
        }

        public void CableAttack()
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
                HandManager.Instance.UseItem(this);
            }
        }

        private void UpdateIndicatorColor()
        {
            if (!Online || Temperature.Overheated() || HasVirus)
            {
                SetIndicatorColor(Color.red);
            }
            else if (CurrentUsers >= UserThreshold * UserCapacity)
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