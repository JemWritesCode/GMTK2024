using UnityEngine;

namespace GameJam {
    public class CableEndPoint : MonoBehaviour
    {
        public CableStartPoint Connection;

        public bool IsConnected()
        {
            return Connection != null;
        }

        public void CancelConnection()
        {
            CableManager.Instance.CurrentCable = Connection;
            Connection.RemoveConnection();
            Connection = null;
        }

        public void CompleteConnection(CableStartPoint cable)
        {
            Connection = cable;
            cable.CompleteConnection(this);
        }

        public void CableInteract(GameObject interactAgent)
        {
            Debug.Log("Interact With End Cable Box...");
            if (CableManager.Instance.HoldingCable())
            {
                if (!IsConnected())
                {
                    Debug.Log("Box is not connected, connecting cable");
                    CompleteConnection(CableManager.Instance.CurrentCable);
                }
                else
                {
                    Debug.Log("Box is already connected, doing nothing");
                }
            }
            else
            {
                if (IsConnected())
                {
                    Debug.Log("Box is already connected, not holding cable, try to pickup");
                    CancelConnection();
                }
                else
                {
                    Debug.Log("Box is not connected, not holding cable, do nothing");
                }
            }
        }
    }
}
