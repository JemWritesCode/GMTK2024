using UnityEngine;

namespace GameJam
{
    public class CableStartPoint : MonoBehaviour
    {
        public CableEndPoint Connection;
        public LineRenderer line;
        public CableType.CableBoxType Type = CableType.CableBoxType.None;
        public Material CableMaterial;
        public GameObject Hamster;
        public bool HasHamster = false;
        public GameObject CableAttachPoint;
        public float CableWidth = 0.025f;


        // SFX
        AudioSource cableStartPointAudioSource;
        public AudioClip grabCableSound;
        public AudioClip hamsterNibbleSound;

        private bool pickedUp = false;

        void Start()
        {
            if (line == null)
            {
                line = gameObject.AddComponent<LineRenderer>();
            }

            line.SetPosition(0, GetCableAttachPoint());
            line.SetPosition(1, GetCableAttachPoint());
            line.material = CableMaterial;
            line.startWidth = CableWidth;
            line.endWidth = CableWidth;
            line.useWorldSpace = true;

            cableStartPointAudioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (pickedUp)
            {
                var transform = InteractManager.Instance.InteractAgent.transform;
                line.SetPosition(1, transform.forward + transform.position);
            }
            else
            {
                RedrawCable();
            }
        }

        public Vector3 GetCableAttachPoint()
        {
            if (CableAttachPoint == null)
            {
                return this.transform.position;
            }

            return CableAttachPoint.transform.position;
        }

        public void BreakConnection()
        {
            if (Connection != null)
            {
                Connection.Connection = null;
                Connection = null;
            }

            pickedUp = false;
            line.SetPosition(1, GetCableAttachPoint());
        }

        public void CancelConnection()
        {
            BreakConnection();
            HandManager.Instance.CurrentCable = null;
        }

        public void StartConnection()
        {
            HandManager.Instance.CurrentCable = this;
            BreakConnection();
            pickedUp = true;
            if (grabCableSound && cableStartPointAudioSource) {
                cableStartPointAudioSource.PlayOneShot(grabCableSound);
            }
        }

        public void CompleteConnection(CableEndPoint cable)
        {
            pickedUp = false;
            HandManager.Instance.CurrentCable = null;
            Connection = cable;
            line.SetPosition(1, Connection.GetCableAttachPoint());
        }

        public void RedrawCable()
        {
            if (IsConnected())
            {
                line.SetPosition(1, Connection.GetCableAttachPoint());
            }
        }

        public bool IsConnected()
        {
            return Connection != null;
        }

        public void CableInteract(GameObject interactAgent)
        {
            if (HandManager.Instance.TryPickup(this))
            {
                if (!IsConnected())
                {
                    StartConnection();
                }
            }
            else if (HandManager.Instance.CurrentCable == this)
            {
                CancelConnection();
                return;
            }
        }

        public void HamsterAttack()
        {
            if (Hamster != null)
            {
                var obj = GameObject.Instantiate(Hamster, this.transform.position,
                    this.transform.rotation * Hamster.transform.rotation);
                obj.transform.position = transform.position;
                obj.GetComponent<Hamster>().Lunch = this;
                HasHamster = true;
                if (hamsterNibbleSound && cableStartPointAudioSource)
                {
                    cableStartPointAudioSource.PlayOneShot(hamsterNibbleSound, .4f);
                }
            }
        }
    }
}
