using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GameJam
{
    public class LevelController : MonoBehaviour
    {
        [System.Serializable]
        public class Level
        {
            [SerializeField]
            public GameObject ParentObject;

            [SerializeField]
            public int UsersNeededForLevel = 0;

            [SerializeField]
            public UnityEvent Event;
        }

        public List<Level> Levels = new List<Level>();
        public int CurrentLevel = 0;
        public int TotalUsers = 0;

        public List<FireWall> FireWalls = new List<FireWall>();
        public List<Server> Servers = new List<Server>();
        public List<CableStartPoint> PowerBoxes = new List<CableStartPoint>();

        public bool EnableHeat = false;
        public bool EnableVirusAttacks = false;
        public bool EnableHamsterAttacks = false;
        public bool EnableSneezeAttacks = false;

        private readonly float updateInterval = 5f;
        private float updateTimer = 0f;

        private void Awake()
        {
            RefreshRoom();
        }

        private void Update()
        {
            // TODO pause this update when in some UI states
            if (Levels == null)
            {
                return;
            }

            UpdateLevelState();
            PeriodicUpdate();
        }

        public void PeriodicUpdate()
        {
            var time = Time.time;
            var delta = time - updateTimer;

            if (delta < updateInterval)
            {
                return;
            }

            if ((CurrentLevel + 1) < Levels.Count && TotalUsers >= Levels[CurrentLevel + 1].UsersNeededForLevel)
            {
                CurrentLevel++;
                RefreshRoom();
            }

            foreach (var server in Servers)
            {
                server.ServeServer(EnableHeat);
            }

            if (EnableVirusAttacks)
            {
                RandomVirusAttack();
            }

            if (EnableHamsterAttacks)
            {
                RandomHamsterAttack();
            }

            if (EnableSneezeAttacks)
            {
                RandomSneezeAttack();
            }

            updateTimer = time;
        }

        public void UpdateLevelState()
        {
            int totalUsers = GetTotalUsers();

            if (TotalUsers != totalUsers) {
                GameManager.Instance.SetUserCount(totalUsers);
                TotalUsers = totalUsers;
            }
        }

        public int GetTotalUsers()
        {
            if (Servers == null)
            {
                return TotalUsers;
            }

            int totalUsers = 0;

            foreach (Server server in Servers)
            {
                totalUsers += server.CurrentUsers;
            }

            return totalUsers;
        }

        public void RefreshRoom()
        {
            for (int lcv = 0; lcv < Levels.Count; lcv++)
            {
                ActivateLevel(lcv, lcv <= CurrentLevel);
            }

            Servers = GetActiveObjectsOfType<Server>();
            FireWalls = GetActiveObjectsOfType<FireWall>();
            PowerBoxes = GetActiveObjectsOfType<CableStartPoint>()
                .Where(cable => cable.Type == CableType.CableBoxType.Power)
                .ToList();
            Debug.Log($"Refreshing Room, {Servers.Count} servers, " +
                $"{FireWalls.Count} firewalls, and {PowerBoxes.Count} power boxes found.");
        }

        public void ActivateLevel(int index, bool activate)
        {
            if (Levels != null && Levels.Count > index && index >= 0)
            {
                Levels[index].ParentObject.SetActive(activate);

                if (activate)
                {
                    Levels[index].Event?.Invoke();
                }
            }
        }

        public List<T> GetActiveObjectsOfType<T>()
        {
            List<T> list = new List<T>();
            foreach (var level in Levels)
            {
                if (level.ParentObject.activeSelf)
                {
                    var objects = level.ParentObject.GetComponentsInChildren<T>();
                    list.AddRange(objects);
                }
            }

            return list;
        }

        public void EventEnableHeat()
        {
            Debug.Log("Enabling Heat! Feeling Hot Hot Hot!");
            EnableHeat = true;
        }

        public void EventEnableVirusAttacks()
        {
            Debug.Log("Enabling Viruses! Get Well Soon!");
            EnableVirusAttacks = true;
        }

        public void RandomVirusAttack()
        {
            if (FireWalls == null || Servers == null)
            {
                return;
            }

            foreach (var firewall in FireWalls)
            {
                if (!firewall.TryBlockAttack())
                {
                    var servers = Servers.Where(item => item.IsOnline()).ToList();
                    if (servers.Count > 0)
                    {
                        int index = UnityEngine.Random.Range(0, servers.Count);
                        Debug.Log($"ATTACK on server {index}!!");
                        servers[index].SetVirus(true);
                    }
                }
            }
        }

        public void EventEnableHamsterAttacks()
        {
            Debug.Log("Enabling Rodents!");
            EnableHamsterAttacks = true;
        }

        public void RandomHamsterAttack()
        {
            if (PowerBoxes == null || PowerBoxes.Count == 0)
            {
                return;
            }

            if (UnityEngine.Random.Range(0, 1f) < 0.2f)
            {
                var boxes = PowerBoxes.Where(item => item.IsConnected() && !item.HasHamster).ToList();
                if (boxes.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, boxes.Count);
                    Debug.Log($"Hamster ATTACK on power box {index}!!");
                    boxes[index].HamsterAttack();
                }
            }
        }

        public void EventDropPackets()
        {
            Debug.Log("Enabling Dropping Packets! OwO OwO");
            // TODO
        }

        public void EventEnableSneezeAttacks()
        {
            EnableSneezeAttacks = true;
        }

        public void RandomSneezeAttack()
        {
            Debug.Log("Enabling Virus Overload! Achooooo!");
            // TODO
        }
    }
}