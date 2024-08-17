using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class CableManager : SingletonManager<CableManager> {
    [field: SerializeField]
    public CableConnection CurrentCable { get; set; }
  }
}
