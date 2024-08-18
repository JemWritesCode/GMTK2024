using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class LevelController : MonoBehaviour
    {
        public int CurrentLevel = 0;
        public List<GameObject> Levels = new List<GameObject>();
        public List<int> UsersNeededForLevel = new List<int>();

        private List<CommandCenter> commandCenters = new List<CommandCenter>();

        private void Awake()
        {
            RefreshRoom();
        }

        private void Update()
        {
            if (commandCenters == null || Levels.Count != UsersNeededForLevel.Count)
            {
                return;
            }

            int totalUsers = 0;
            foreach (var c in commandCenters)
            {
                totalUsers += c.Users;
            }

            if (totalUsers >= UsersNeededForLevel[CurrentLevel + 1])
            {
                CurrentLevel++;
                RefreshRoom();
            }
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
                Levels[index].SetActive(activate);

                commandCenters = GetActiveObjectsOfType<CommandCenter>();
                var servers = GetActiveObjectsOfType<Server>();
                var firewalls = GetActiveObjectsOfType<FireWall>();

                // Treat all command centers as the same object
                // Will likely need to change this to support 1 or multiple
                // Users are currently tied to command centers
                foreach (var c in commandCenters)
                {
                    c.Servers = servers;
                    c.FireWalls = firewalls;
                }
            }
        }

        public List<T> GetActiveObjectsOfType<T>()
        {
            List<T> list = new List<T>();
            foreach (var level in Levels)
            {
                if (level.activeSelf)
                {
                    var objects = level.GetComponentsInChildren<T>();
                    list.AddRange(objects);
                }
            }

            return list;
        }
    }
}