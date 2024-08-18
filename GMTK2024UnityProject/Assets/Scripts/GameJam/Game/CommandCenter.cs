using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameJam
{
    public class CommandCenter : MonoBehaviour
    {
        public List<FireWall> FireWalls = new List<FireWall>();
        public List<Server> Servers = new List<Server>();
        public List<CableStartPoint> PowerBoxes = new List<CableStartPoint>();
        public int Users = 0;

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
            foreach (Server server in Servers)
            {
                users += server.ServeServer();
            }

            RandomVirusAttack();
            //RandomHamsterAttack();

            updateTimer = time;
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
                        int index = Random.Range(0, servers.Count);
                        Debug.Log($"ATTACK on server {index}!!");
                        servers[index].SetVirus(true);
                    }
                }
            }
        }

        public void RandomHamsterAttack()
        {
            if (PowerBoxes == null)
            {
                return;
            }

            if (PowerBoxes.Count > 0)
            {
                var boxes = PowerBoxes.Where(item => item.IsConnected() && !item.HasHamster).ToList();
                if (boxes.Count > 0)
                {
                    int index = Random.Range(0, boxes.Count);
                    Debug.Log($"Hamster ATTACK on power box {index}!!");
                    boxes[index].HamsterAttack();
                }
            }
        }
    }
}