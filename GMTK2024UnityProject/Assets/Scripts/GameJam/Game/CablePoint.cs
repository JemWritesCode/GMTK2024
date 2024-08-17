using UnityEngine;

namespace GameJam {
    public class CablePoint : MonoBehaviour
    {
        public bool CanRemove = false;
        public CablePoint Connection;
        public CableConnection Cable;

        public void Pickup()
        {
            Connection = null;
        }

        public void SetConnection(CablePoint point)
        {
            Connection = point;
        }

        public bool IsConnected()
        {
            return Connection != null;
        }

        public void CableInteract(GameObject interactAgent)
        {
            Debug.Log("Interact Yeee");
            if (Connection != null)
            {
                // Already has connected cable box
                if (CanRemove)
                {
                    Player.CurrentCable = Connection.Cable;
                    Connection.SetConnection(null);
                    Connection = null;
                }
            }
            else
            {
                // Check if player has cable in hand
                if (Player.CurrentCable != null)
                {
                    // If so connect that cable to this box
                    Player.CurrentCable.EndConnection(this);
                }
                else
                {
                    // If not start a connection
                    if (Cable == null)
                    {
                        CableConnection cable = this.gameObject.AddComponent<CableConnection>();
                        cable.StartConnection();
                        Player.CurrentCable = cable;
                    }
                }
            }
        }
    }
}
