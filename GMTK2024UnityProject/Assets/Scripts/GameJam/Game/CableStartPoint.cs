using UnityEngine;

namespace GameJam
{
    public class CableStartPoint : MonoBehaviour
    {
        public CableEndPoint Connection;
        public LineRenderer line;
        public CableType.CableBoxType Type = CableType.CableBoxType.None;
        public Material CableMaterial;

        private bool pickedUp = false;

        private Vector3 cableOffset = new Vector3 (0, 0.075f, 0);

        void Start()
        {
            if (line == null)
            {
                line = gameObject.AddComponent<LineRenderer>();
            }

            line.SetPosition(0, this.transform.position + cableOffset);
            line.SetPosition(1, this.transform.position + cableOffset);
            line.material = CableMaterial;
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            line.useWorldSpace = true;
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
            pickedUp = false;
            Connection = null;
            line.SetPosition(1, this.transform.position + cableOffset);
        }

        public void MoveConnection()
        {
            Connection = null;
            StartConnection();
        }

        public void CancelConnection()
        {
            pickedUp = false;
            Connection = null;
            HandManager.Instance.CurrentCable = null;
            line.SetPosition(1, this.transform.position + cableOffset);
        }

        public void StartConnection()
        {
            HandManager.Instance.CurrentCable = this;
            Connection = null;
            pickedUp = true;
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
    }
}
