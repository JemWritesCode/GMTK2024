using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace GameJam
{
    public class LevelController : MonoBehaviour
    {
        public List<Level> Levels = new List<Level>();
        public int CurrentLevel = 0;
        public int TotalUsers = 0;

        [System.Serializable]
        public class Level
        {
            [SerializeField]
            public GameObject ParentObject;

            [SerializeField]
            public int UsersNeededForLevel = 0;
            // Game event at level start
        }

        private List<CommandCenter> commandCenters = new List<CommandCenter>();

        private void Awake()
        {
            RefreshRoom();
        }

        private void Update()
        {
            if (commandCenters == null || Levels == null)
            {
                return;
            }

            UpdateLevelState();
        }

        public void UpdateLevelState() {
            int totalUsers = GetTotalUsers();

            if (TotalUsers != totalUsers) {
                GameManager.Instance.SetUserCount(totalUsers);
                TotalUsers = totalUsers;
            }

            if ((CurrentLevel + 1) < Levels.Count && TotalUsers >= Levels[CurrentLevel + 1].UsersNeededForLevel) {
                CurrentLevel++;
                RefreshRoom();
            }
        }

        public int GetTotalUsers() {
            int totalUsers = 0;

            foreach (CommandCenter center in commandCenters) {
                totalUsers += center.Users;
            }

            return totalUsers;
        }

        public void RefreshRoom()
        {
            for (int lcv = 0; lcv < Levels.Count; lcv++)
            {
                ActivateLevel(lcv, lcv <= CurrentLevel);
            }
        }

        public void ActivateLevel(int index, bool activate)
        {
            if (Levels != null && Levels.Count > index && index >= 0)
            {
                Levels[index].ParentObject.SetActive(activate);

                commandCenters = GetActiveObjectsOfType<CommandCenter>();
                var servers = GetActiveObjectsOfType<Server>();
                var firewalls = GetActiveObjectsOfType<FireWall>();
                var powerBoxes = GetActiveObjectsOfType<CableStartPoint>()
                    .Where(cable => cable.Type == CableType.CableBoxType.Power)
                    .ToList();

                Debug.Log($"Activating level {index}, {servers.Count} servers, " +
                    $"{firewalls.Count} firewalls, and {powerBoxes.Count} power boxes found.");

                // Treat all command centers as the same object
                // Will likely need to change this to support 1 or multiple
                // Users are currently tied to command centers
                foreach (var c in commandCenters)
                {
                    c.Servers = servers;
                    c.FireWalls = firewalls;
                    c.PowerBoxes = powerBoxes;
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
    }
}