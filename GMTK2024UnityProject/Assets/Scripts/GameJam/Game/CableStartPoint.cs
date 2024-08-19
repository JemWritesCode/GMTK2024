using UnityEditor.MemoryProfiler;
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

        private bool pickedUp = false;

        private Vector3 cableOffset = new Vector3 (0, 0.075f, 0);

        public float CableWidth = 0.025f;

        public AudioClip grabCableSound;
        AudioSource grabCableAudioSource;

        void Start()
        {
            if (line == null)
            {
                line = gameObject.AddComponent<LineRenderer>();
            }

            line.SetPosition(0, this.transform.position + cableOffset);
            line.SetPosition(1, this.transform.position + cableOffset);
            line.material = CableMaterial;
            line.startWidth = CableWidth;
            line.endWidth = CableWidth;
            line.useWorldSpace = true;

            grabCableAudioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (pickedUp)
            {
                var transform = InteractManager.Instance.InteractAgent.transform;
                line.SetPosition(1, transform.forward + transform.position);
            }
        }

        public void BreakConnection()
        {
            if (Connection != null)
            {
                Connection.Connection = null;
                Connection = null;
            }

            pickedUp = false;
            line.SetPosition(1, this.transform.position + cableOffset);
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
            if (grabCableSound && grabCableAudioSource) {
                grabCableAudioSource.PlayOneShot(grabCableSound);
            }
        }

        public void CompleteConnection(CableEndPoint cable)
        {
            pickedUp = false;
            HandManager.Instance.CurrentCable = null;
            Connection = cable;
            line.SetPosition(1, cable.transform.position + cableOffset);
        }

        public bool IsConnected()
        {
            return Connection != null;
        }

        public void CableInteract(GameObject interactAgent)
        {
            Debug.Log("Interact With Start Cable Box...");

            if (HandManager.Instance.TryPickup(this))
            {
                if (!IsConnected())
                {
                    Debug.Log("Box is not connected, not holding cable, creating one if able");
                    StartConnection();
                }
            }
            else if (HandManager.Instance.CurrentCable == this)
            {
                Debug.Log("Drop cable!");
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
            }
        }
    }
}
