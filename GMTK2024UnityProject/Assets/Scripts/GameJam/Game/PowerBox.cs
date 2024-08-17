using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class PowerBox : MonoBehaviour
    {
        public List<CableStartPoint> Connections = new List<CableStartPoint>();
        public int capacity = 100;

        public int GetPower()
        {
            int connections = 0;
            foreach (CableStartPoint p in Connections)
            {
                if (p.IsConnected())
                {
                    connections++;
                }
            }

            if (connections == 0)
            {
                return 0;
            }

            // TODO make smart
            return (int)(capacity / connections);
        }

        // IDEAS:
        // Able to smartly distribute power as an upgrade
        // Power upgrade to increase capacity "Smart Power"
    }
}