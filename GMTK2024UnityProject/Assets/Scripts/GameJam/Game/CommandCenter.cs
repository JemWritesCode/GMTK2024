using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameJam
{
    public class CommandCenter : MonoBehaviour
    {
        public List<FireWall> FireWalls = new List<FireWall>();
        public List<Server> Servers = new List<Server>();
        public List<Ads.Ad> CurrentAds = new List<Ads.Ad>();
        public int Users = 0;
        public int Reputation = 1000;
        public int Cash = 100;

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
                users -= server.ServeServer(users);
            }

            if (users <= 0)
            {
                // All were served yay
                Reputation += 10;
            }
            else
            {
                Reputation -= 50;
            }


            RandomVirusAttack(Reputation);

            // TODO: add mechanic to increase user count
            AddUsers(Random.Range(1, 10));

            ProcessAds();

            updateTimer = time;
        }

        public void AddUsers(int users)
        {
            Users += users;
        }

        public void RemoveUsers(int users)
        {
            Users -= users;
        }

        public void ProcessAds()
        {
            foreach (Ads.Ad ad in CurrentAds)
            {
                Cash += ad.Revenue;
                Reputation += ad.Reputation;
            }
        }

        public void AddAd(Ads.Ad ad)
        {
            CurrentAds.Add(ad);
        }

        public void SpendCash(int cash)
        {
            Cash -= cash;
        }

        public void RandomVirusAttack(int reputation)
        {
            if (FireWalls == null || Servers == null)
            {
                return;
            }

            foreach (var firewall in FireWalls)
            {
                if (!firewall.TryBlockAttack(reputation))
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

        public void OpenUI()
        {
            // TODO
        }
    }
}