using UnityEngine;
using static GameJam.CableType;

namespace GameJam
{
    public class CableEndPoint : MonoBehaviour
    {
        public CableStartPoint Connection;
        public CableBoxType Type = CableBoxType.None;
        public AudioClip cableEndPointSound;
        public AudioClip wrongCableTypeSound;
        AudioSource cableEndPointAudioSource;

        public GameObject CableAttachPoint;

        private void Start()
        {
            cableEndPointAudioSource = GetComponent<AudioSource>();
        }

        public Vector3 GetCableAttachPoint()
        {
            if (CableAttachPoint == null)
            {
                return this.transform.position;
            }

            return CableAttachPoint.transform.position;
        }

        public bool IsConnected()
        {
            return Connection != null;
        }

        public void BreakConnection()
        {
            if (IsConnected())
            {
                Connection.BreakConnection();
            }
        }

        public void CancelConnection()
        {
            if (IsConnected())
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
            if (HandManager.Instance.HoldingCable())
            {

                if (!IsConnected() && HandManager.Instance.CurrentCable.Type == Type)
                {
                    CompleteConnection(HandManager.Instance.CurrentCable);
                }
                else if (!(HandManager.Instance.CurrentCable.Type == Type))
                {
                    cableEndPointAudioSource.PlayOneShot(wrongCableTypeSound, .5f);
                }
            }
            else if (IsConnected())
            {
                CancelConnection();
            }
        }
    }
}
