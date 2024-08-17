using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class PowerBox : MonoBehaviour
    {
        public List<CableStartPoint> Connections = new List<CableStartPoint>();
        public float capacity = 100f;

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
            // TODO make smart
            Debug.Log($"Power box has {connections} connections");
            return (int)(capacity / connections);
        }

        // IDEAS:
        // Able to smartly distribute power as an upgrade
        // Power upgrade to increase capacity "Smart Power"
    }
}