using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

namespace GameJam
{
    public class Server : MonoBehaviour
    {
        public LevelController levelController;

        [Header("Live Populated Fields, Don't Touch")]
        public int CurrentUsers = 0;
        public int PowerMultiplier = 0;
        public bool Online = false;
        public bool HasVirus = false;

        public List<CableEndPoint> PowerConnections = new List<CableEndPoint>();
        public List<CableEndPoint> DataConnections = new List<CableEndPoint>();

        public Temperature Temperature = new Temperature();

        [Header("Effects")]
        public GameObject SmokeEffects;
        public GameObject FireEffects;
        public GameObject VirusIndicator;
        public float VirusInterval = 0.75f;
        private float VirusTimer;
        private bool VirusPosition = false;

        [Header("Lights")]
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
            /*if (PowerMultiplier == 0)
            {
                if (IndicatorLights != null)
                {
                    foreach (var light in IndicatorLights)
                    {
                        light.enabled = false;
                    }
                }

                if (VirusIndicator != null)
                {
                    VirusIndicator.gameObject.SetActive(false);
                }

                return;
            }*/

            if (HasVirus && IndicatorLights != null)
            {
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

            if (VirusIndicator != null)
            {
                VirusIndicator.gameObject.SetActive(HasVirus);

                if (HasVirus)
                {
                    VirusTimer += Time.deltaTime;
                    if (VirusTimer > VirusInterval)
                    {
                        if (VirusPosition)
                        {
                            VirusPosition = !VirusPosition;
                            VirusIndicator.transform.position += new Vector3(0, 0.1f, 0);
                        }
                        else
                        {
                            VirusPosition = !VirusPosition;
                            VirusIndicator.transform.position += new Vector3(0, -0.1f, 0);
                        }
                        VirusTimer = 0;
                    }
                }
            }
        }

        public int ServeServer(bool heatEnabled)
        {
            bool overheated = Temperature.Overheated();
            Temperature.PlayEffects(FireEffects, SmokeEffects);

            var dataConnections = DataConnections.Where(data => data.IsConnected()).ToList();
            var powerConnections = PowerConnections.Where(data => data.IsConnected()).ToList();

            if (dataConnections.Count == 0 || powerConnections.Count == 0 || overheated || HasVirus)
            {
                SetOnline(false);
                SetCurrentUsers(0);
                SetPowerMultiplier(powerConnections.Count);

                // TODO balance
                if (overheated)
                {
                    CableAttackChance(powerConnections, levelController.PowerCableDisconnectChance, true);
                }

                if (HasVirus)
                {
                    CableAttackChance(dataConnections, levelController.DataCableDisconnectChance, true);
                }
            }
            else
            {
                SetOnline(true);
                SetCurrentUsers(dataConnections.Count * powerConnections.Count * levelController.Levels[levelController.CurrentLevel].UsersPerPortAtLevel);
                SetPowerMultiplier(powerConnections.Count);

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
            if (TryGetComponent(out Yeet yeet))
            {
                // If we want to reenable this need to make sure fire effects on cables don't play
                //yeet.StartYeet(() => RandomCableAttack(dataCablePercent, powerCablePercent));
                yeet.StartYeet(null);
            }
        }

        public void CableAttackChance(List<CableEndPoint> connections, float percentage, bool playEffects)
        {
            if (UnityEngine.Random.Range(0f, 1f) <= percentage)
            {
                CableAttack(connections, 1, playEffects);
            }
        }

        public void CableAttack(List<CableEndPoint> connections, int count, bool playEffects)
        {
            if (connections.Count > 0)
            {
                var list = SelectRandomItems(connections, count);

                foreach (var cable in list)
                {
                    cable.BreakConnection(playEffects);
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
            if (HasVirus)
            {
                SetIndicatorColor(new Color(0.616f, 0, 1));
            }
            else if (!Online || Temperature.Overheated())
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