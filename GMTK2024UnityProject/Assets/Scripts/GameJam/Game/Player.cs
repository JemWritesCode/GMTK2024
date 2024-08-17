using UnityEngine;

namespace GameJam
{
    public class Player : MonoBehaviour
    {
        static Player() {}

        private static readonly Player instance = new Player();

        public static Player Instance
        {
            get => instance;
        }

        public static CableConnection CurrentCable;
    }
}
