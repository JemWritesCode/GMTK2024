using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

namespace GameJam
{
    public class Server : MonoBehaviour
    {
        public int CurrentUsers = 0;
        public int PowerMultiplier = 0;
        public bool Online = false;
        public bool HasVirus = false;

        public LevelController levelController;

        public List<CableEndPoint> PowerConnections = new List<CableEndPoint>();
        public List<CableEndPoint> DataConnections = new List<CableEndPoint>();

        public Temperature Temperature = new Temperature();

        public GameObject FireEffects;

        public List<Light> IndicatorLights;
        public float LightBlinkInterval = 0.5f;
        private float lightBlinkTimer;

        [field: Header("Events")]
        [field: SerializeField]
        public UnityEvent<Server> ServerWasServed { get; set; }

        private void Awake()
        {
            var cablePoints = this.GetComponentsInChildren<CableEndPoint>();

            PowerConnections = cablePoints.Where(cable => cable.Type == CableType.CableBoxType.Power).ToList();
            DataConnections = cablePoints.Where(cable => cable.Type == CableType.CableBoxType.Data).ToList();

            Debug.Log($"Setting up Server, {PowerConnections.Count} power connections " +
                $"and {DataConnections.Count} data connections found.");
        }

        private void Update()
        {
            if (IndicatorLights == null || !HasVirus)
            {
                return;
            }

            lightBlinkTimer += Time.deltaTime;
            if (lightBlinkTimer > LightBlinkInterval)
            {
                foreach (var light in IndicatorLights)
                {
                    light.enabled = !light.enabled;
                }
                lightBlinkTimer = 0;
            }
        }

        public int ServeServer(bool heatEnabled)
        {
            if (Temperature.Overheated())
            {
                FireEffects.SetActive(true);
            }
            else
            {
                FireEffects.SetActive(false);
            }

            int dataConnections = DataConnections.Where(data => data.IsConnected()).Count();
            int powerConnections = PowerConnections.Where(data => data.IsConnected()).Count();

            if (dataConnections == 0 || powerConnections == 0 || Temperature.Overheated() || HasVirus)
            {
                SetOnline(false);
                SetCurrentUsers(0);
                SetPowerMultiplier(powerConnections);
            }
            else
            {
                SetOnline(true);
                SetCurrentUsers(dataConnections * powerConnections * levelController.Levels[levelController.CurrentLevel].UsersPerPortAtLevel);
                SetPowerMultiplier(powerConnections);

                if (heatEnabled)
                {
                    Temperature.UpdateHeat();
                }
            }

            UpdateIndicatorColor();
            ServerWasServed.Invoke(this);

            return CurrentUsers;
        }

        public void SetOnline(bool online)
        {
            Online = online;
        }

        public bool IsOnline()
        {
            return Online && !HasVirus;
        }

        public void SetCurrentUsers(int currentUsers) { 
          CurrentUsers = currentUsers;
        }

        public void SetPowerMultiplier(int powerMultiplier) {
          PowerMultiplier = powerMultiplier;
        }

        public void SetVirus(bool virus)
        {
            HasVirus = virus;
            UpdateIndicatorColor();

            if (!HasVirus)
            {
                foreach (var light in IndicatorLights)
                {
                    light.enabled = true;
                }
            }
        }

        public void Sneeze(float dataCablePercent, float powerCablePercent)
        {
            var yeet = GetComponent<Yeet>();
            if (yeet != null)
            {
                yeet.StartYeet(() => RandomCableAttack(dataCablePercent, powerCablePercent));
            }
        }

        public void PowerCableAttack(float percentage)
        {
            CableAttack(PowerConnections, percentage);
        }

        public void DataCableAttack(float percentage)
        {
            CableAttack(DataConnections, percentage);
        }

        public void RandomCableAttack(float dataCablePercent, float powerCablePercent)
        {
            CableAttack(DataConnections, dataCablePercent);
            CableAttack(PowerConnections, powerCablePercent);
        }

        public void CableAttack(List<CableEndPoint> connections, float percentage)
        {
            if (connections.Count > 0)
            {
                int count = Math.Clamp((int)(connections.Count * percentage), 1, connections.Count);
                var list = SelectRandomItems(connections, count);

                foreach (var cable in list)
                {
                    cable.BreakConnection();
                }
            }
        }

        private List<T> SelectRandomItems<T>(List<T> list, int n)
        {
            List<T> results = new List<T>();
            var random = new System.Random();
            HashSet<int> indexes = new HashSet<int>();
            if (n > list.Count)
            {
                // Return all items
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    results.Add(item);
                    indexes.Add(i);
                }
            }
            else
            {
                int count = 0;

                while (count < n)
                {
                    var index = random.Next(0, list.Count);
                    if (!indexes.Contains(index))
                    {
                        var item = list[index];
                        results.Add(item);
                        indexes.Add(index);
                        count++;
                    }
                }
            }

            return results;
        }

        public void ServerInteract(GameObject interactAgent)
        {
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
            foreach (var light in IndicatorLights)
            {
                light.color = color;
            }
        }
    }
}