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

        void Start()
        {
            if (line == null)
            {
                line = gameObject.AddComponent<LineRenderer>();
            }

            line.SetPosition(0, this.transform.position);
            line.SetPosition(1, this.transform.position);
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

        public void RemoveConnection()
        {
            Connection = null;
            StartConnection();
        }

        public void CancelConnection()
        {
            pickedUp = false;
            HandManager.Instance.CurrentCable = null;
            line.SetPosition(1, this.transform.position);
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
            line.SetPosition(1, cable.transform.position);
        }

        public bool IsConnected()
        {
            return Connection != null;
        }

        public void CableInteract(GameObject interactAgent)
        {
            Debug.Log("Interact With Start Cable Box...");
            if (HandManager.Instance.HoldingCable())
            {
                if (HandManager.Instance.CurrentCable == this)
                {
                    Debug.Log("Drop cable!");
                    CancelConnection();
                    return;
                }


                Debug.Log("Box is a starting box, holding a cable, doing nothing");
            }
            else
            {
                if (!IsConnected())
                {
                    Debug.Log("Box is not connected, not holding cable, creating one if able");
                    StartConnection();
                }
                else
                {
                    Debug.Log("Box is already connected, not holding cable, do nothing");
                }
            }
        }
    }
}
