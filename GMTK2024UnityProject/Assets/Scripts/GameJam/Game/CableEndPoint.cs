using UnityEngine;
using static GameJam.CableType;

namespace GameJam
{
    public class CableEndPoint : MonoBehaviour
    {
        public CableStartPoint Connection;
        public CableBoxType Type = CableBoxType.None;
        public AudioClip cableEndPointSound;
        AudioSource cableEndPointAudioSource;

        private void Start()
        {
            cableEndPointAudioSource = GetComponent<AudioSource>();
        }

        public bool IsConnected()
        {
            return Connection != null;
        }

        public void BreakConnection()
        {
            if (Connection != null)
            {
                Connection.Connection = null;
                Connection = null;
            }
        }

        public void CancelConnection()
        {
            if (Connection != null)
            {
                Connection.StartConnection();
                Connection = null;
            }
        }

        public void CompleteConnection(CableStartPoint cable)
        {
            Connection = cable;
            cable.CompleteConnection(this);
            if (cableEndPointSound && cableEndPointAudioSource)
            {
                cableEndPointAudioSource.PlayOneShot(cableEndPointSound);
            }
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
