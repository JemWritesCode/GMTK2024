using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Ads : MonoBehaviour
    {
        public class Ad
        {
            public int Revenue = 10;
            public int Reputation = -2;
            public string Company = "Ad company";
            public string Message = "Buy stuff";
        }

        public List<Ad> AdList = new List<Ad>();

        public Ad GetRandomAd()
        {
            int random = Random.Range(0, AdList.Count);
            return AdList[random];
        }
    }
}