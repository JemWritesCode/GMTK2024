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

        public Light IndicatorLight;
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
            if (!IndicatorLight || !HasVirus)
            {
                return;
            }

            lightBlinkTimer += Time.deltaTime;
            if (lightBlinkTimer > LightBlinkInterval)
            {
                IndicatorLight.enabled = !IndicatorLight.enabled;
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
                IndicatorLight.enabled = true;
            }
        }

        public void CableAttack()
        {
            // TODO support more kinds of attacks, make this better
            if (UnityEngine.Random.Range(0, 1) == 0)
            {
                var connections = PowerConnections.Where(item => item.IsConnected()).ToList();
                if (connections.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, connections.Count);
                    connections[index].BreakConnection();
                }
            }
            else
            {
                var connections = DataConnections.Where(item => item.IsConnected()).ToList();
                if (connections.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, connections.Count);
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