using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class CommandCenter : MonoBehaviour
    {
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

            if (Reputation < 0)
            {
                RandomAttack();
            }

            // TODO: add mechanic to increase user count
            AddUsers(Random.Range(1, 10));

            ProcessAds();

            updateTimer = time;
        }

        public void AddServer(Server server)
        {
            Servers.Add(server);
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

        public void RandomAttack()
        {
            int index = Random.Range(1, Servers.Count);
            Servers[index].RandomAttack();
        }

        public void OpenUI()
        {
            // TODO
        }
    }
}