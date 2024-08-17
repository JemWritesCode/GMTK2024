using UnityEngine;

using YoloBox;

namespace GameJam
{
    public sealed class CableManager : SingletonManager<CableManager>
    {
        [field: SerializeField]
        public CableStartPoint CurrentCable { get; set; }

        public bool HoldingCable()
        {
            return CurrentCable != null;
        }
    }
}
