using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class CommandCenter : MonoBehaviour
    {
        private List<Server> servers;
        public int Users = 0;
        public int Reputation = 1000;

        private readonly float updateInterval = 5f;
        private float updateTimer = 0f;

        void Update()
        {
            // TODO get Red to yell at me for this
            var time = Time.time;
            var delta = time - updateTimer;

            if (delta < updateInterval)
            {
                return;
            }

            int users = Users;
            foreach (Server server in servers)
            {
                if (users <= 0)
                {
                    break;
                }

                users -= server.ServeServer(users);
            }

            // TODO: reputation system
            if (users <= 0)
            {
                // All were served yay
                Reputation += 10;
            }
            else
            {
                Reputation -= 100;
            }

            // TODO: add mechanic to increase user count
            AddUsers((int)Random.Range(0.1f, 1f) * 100);

            updateTimer = time;
        }

        public void AddServer(Server server)
        {
            servers.Add(server);
        }

        public void AddUsers(int users)
        {
            Users += users;
        }

        public void RemoveUsers(int users)
        {
            Users -= users;
        }
    }
}