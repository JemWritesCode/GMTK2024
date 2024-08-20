using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace GameJam
{
    public class LevelController : MonoBehaviour
    {
        public List<Level> Levels = new List<Level>();
        public int CurrentLevel = 0;

        [Header("Live Populated Fields, Don't Touch")]
        public int TotalUsers = 0;
        public List<FireWall> FireWalls = new List<FireWall>();
        public List<Server> Servers = new List<Server>();
        public List<CableStartPoint> PowerBoxes = new List<CableStartPoint>();

        public AudioSource levelControllerAudioSource;

        [Header("Heat")]
        public bool EnableHeat = false;
        public float PowerCableDisconnectChance = 0.02f;

        // Viruses
        [Header("Virus")]
        public bool EnableVirusAttacks = false;
        public float DataCableDisconnectChance = 0.02f;
        public AudioClip virusEventSound;
        public float minimumTimeBetweenViruses = 35f;
        public float maximumTimeBetweenViruses = 45f;
        public float timeOfLastVirus = 0f;

        [Header("❤ Hamsters ❤")]
        public bool EnableHamsterAttacks = false;
        public float HamstersPercent = 0.05f;
        public float minimumTimeBetweenHamsters = 30f;
        public float maximumTimeBetweenHamsters = 45f;
        public float timeOfLastHamster = 0f;

        [Header("▰ Drop Packets ▰")]
        public bool DroppedPackets = false;
        public float DropPacketsPercent = 0.5f;

        [Header("ミ Sneeze ミ")]
        public bool EnableSneezeAttacks = false;
        public float SneezeAttackChance = 0.05f; // To be removed, should be set x times?
        public float SneezeDropPercentDataCables = 0.2f;
        public float SneezeDropPercentPowerCables = 0.3f;
        public AudioClip sneezeEventSound;
        private readonly float updateInterval = 1f;
        private float updateTimer = 0f;
        public float minimumTimeBetweenSneezes = 15f;
        public float maximumTimeBetweenSneezes = 45f;
        public float timeOfLastSneeze = 0f;

        private IEnumerator Start()
        {
            yield return null;
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
                if (time > timeOfLastVirus + minimumTimeBetweenViruses)
                {
                    RandomVirusAttack(); 
                }
            }

            if (EnableHamsterAttacks)
            {
                if (time > timeOfLastHamster + minimumTimeBetweenHamsters)
                {
                    RandomHamsterAttack();
                }
            }

            if (EnableSneezeAttacks)
            {
                if (time > timeOfLastSneeze + minimumTimeBetweenSneezes)
                {
                    RandomSneezeAttack(SneezeAttackChance);
                }
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

            timeOfLastVirus = Time.time + minimumTimeBetweenViruses;
            timeOfLastHamster = Time.time + minimumTimeBetweenHamsters;

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
                Level level = Levels[index];
                level.ParentObject.SetActive(activate);

                if (activate)
                {
                    level.Event?.Invoke();

                    if (level.LevelStartDialogNode) {
                      GameManager.Instance.SetDialogNode(level.LevelStartDialogNode);
                    }
                }
            }
        }

        public List<T> GetActiveObjectsOfType<T>() where T : UnityEngine.Component
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
            bool guaranteeVirusAttack = false;
            if (FireWalls == null || Servers == null)
            {
                return;
            }

            if (Time.time > timeOfLastVirus + maximumTimeBetweenViruses)
            {
                guaranteeVirusAttack = true;
            }

            foreach (var firewall in FireWalls)
            {
                if (!firewall.TryBlockAttack() || guaranteeVirusAttack == true)
                {
                    var servers = Servers.Where(item => item.IsOnline()).ToList();
                    if (servers.Count > 0)
                    {
                        int index = UnityEngine.Random.Range(0, servers.Count);
                        Debug.Log($"ATTACK on server {index}!!");
                        servers[index].SetVirus(true);
                        timeOfLastVirus = Time.time;
                        guaranteeVirusAttack = false;
                        if ( levelControllerAudioSource && virusEventSound) 
                        {
                            levelControllerAudioSource.PlayOneShot(virusEventSound, 1f);
                        }
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
            bool guaranteeHamsterAttack = false;
            if (PowerBoxes == null || PowerBoxes.Count == 0)
            {
                return;
            }
            if (Time.time > timeOfLastHamster + maximumTimeBetweenHamsters)
            {
                guaranteeHamsterAttack = true;
            }
            if (UnityEngine.Random.Range(0, 1f) < HamstersPercent || guaranteeHamsterAttack)
            {
                var boxes = PowerBoxes.Where(item => item.IsConnected() && !item.HasHamster).ToList();
                if (boxes.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, boxes.Count);
                    Debug.Log($"Hamster ATTACK on power box {index}!!");
                    boxes[index].HamsterAttack();
                    timeOfLastHamster = Time.time;
                    guaranteeHamsterAttack = false;
                }
            }
        }

        public void EventDropPackets()
        {
            Debug.Log("Enabling Dropping Packets! OwO OwO");
            if (Servers == null || DroppedPackets)
            {
                return;
            }

            foreach (var server in Servers)
            {
                server.CableAttack(server.DataConnections, (int)(server.DataConnections.Count * DropPacketsPercent), true);
            }

            DroppedPackets = true;
        }

        public void EventEnableSneezeAttacks()
        {
            Debug.Log("Enabling Virus Overload! Achooooo!");
            EnableSneezeAttacks = true;
            RandomSneezeAttack(1f);
        }

        public void RandomSneezeAttack(float percentage)
        {
            bool guaranteeSneezeAttack = false;
            if (Servers == null)
            {
                return;
            }
            if (Time.time > timeOfLastSneeze + maximumTimeBetweenSneezes)
            {
                guaranteeSneezeAttack = true;
            }

            if (UnityEngine.Random.Range(0, 1f) <= percentage || guaranteeSneezeAttack)
            {
                foreach (var server in Servers)
                {
                    server.Sneeze(SneezeDropPercentDataCables, SneezeDropPercentPowerCables);
                    timeOfLastSneeze = Time.time;
                    if (levelControllerAudioSource && sneezeEventSound)
                    {
                        levelControllerAudioSource.PlayOneShot(sneezeEventSound, .4f);
                    }
                }
            }
        }
    }
}