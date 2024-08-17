using UnityEngine;

namespace GameJam {
    public class CableConnection : MonoBehaviour
    {
        public LineRenderer line;
        //public CablePoint start;
        public CablePoint end;

        private bool pickedUp = true;

        void Start()
        {
            if (line == null)
            {
                line = gameObject.AddComponent<LineRenderer>();
                line.SetPosition(0, this.transform.position);
                line.SetPosition(1, this.transform.position);
                line.startColor = Color.black;
                line.endColor = Color.black;
                line.startWidth = 0.05f;
                line.endWidth = 0.05f;
                //line.positionCount = 2;
                line.useWorldSpace = true;
            }
        }

        void Update()
        {
            if (pickedUp)
            {
                line.SetPosition(1, Player.Instance.gameObject.transform.position);
            }
        }

        public void StartConnection()
        {
            pickedUp = true;
        }

        public void EndConnection(CablePoint cable)
        {
            pickedUp = false;
            end = cable;
            line.SetPosition(1, cable.transform.position);
        }
    }
}
