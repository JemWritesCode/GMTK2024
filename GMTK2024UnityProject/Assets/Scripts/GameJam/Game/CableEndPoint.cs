using UnityEngine;
using static GameJam.CableType;

namespace GameJam
{
    public class CableEndPoint : MonoBehaviour
    {
        public CableStartPoint Connection;
        public CableBoxType Type = CableBoxType.None;

        public bool IsConnected()
        {
            return Connection != null;
        }

        public void BreakConnection()
        {
            Connection.BreakConnection();
            Connection = null;
        }

        public void CancelConnection()
        {
            HandManager.Instance.CurrentCable = Connection;
            Connection.MoveConnection();
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
            if (HandManager.Instance.HoldingCable())
            {
                if (!IsConnected() && HandManager.Instance.CurrentCable.Type == Type)
                {
                    Debug.Log("Box is not connected, connecting cable");
                    CompleteConnection(HandManager.Instance.CurrentCable);
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
