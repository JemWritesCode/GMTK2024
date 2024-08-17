using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class PowerBox : MonoBehaviour
    {
        public List<CableEndPoint> Connections = new List<CableEndPoint>();
        public float capacity = 100f;

        public int GetPower()
        {
            int connections = 0;
            foreach (CableEndPoint p in Connections)
            {
                if (p.IsConnected())
                {
                    connections++;
                }
            }
            // TODO make smart
            return (int)(capacity / connections);
        }

        // IDEAS:
        // Able to smartly distribute power as an upgrade
        // Power upgrade to increase capacity "Smart Power"
    }
}