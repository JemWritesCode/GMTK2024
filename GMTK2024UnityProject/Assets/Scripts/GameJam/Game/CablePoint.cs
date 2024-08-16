using UnityEngine;

namespace GameJam {
    public class CablePoint : MonoBehaviour
    {
        public bool canRemove = false;
        public CablePoint connection;

        public void Pickup()
        {
            connection = null;
        }

        public void SetConnection(CablePoint point)
        {
            connection = point;
        }

        public bool IsConnected()
        {
            return connection != null;
        }
    }
}
