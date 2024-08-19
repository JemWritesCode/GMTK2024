using UnityEngine;

namespace GameJam {
  public sealed class WorldAdController : MonoBehaviour {
    [field: Header("Ad")]
    [field: SerializeField]
    public Transform AdTransform { get; private set; }

    [field: SerializeField]
    public SpriteRenderer AdSpriteRenderer { get; private set; }

    [field: Header("State")]
    [field: SerializeField]
    public float TimeToLive { get; private set; } = 5f;
  }
}
