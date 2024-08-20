using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class AudioManager : SingletonManager<AudioManager> {
    [field: Header("Volumes")]
    [field: SerializeField]
    public float AudioVolume { get; private set; } = 0.75f;

    [field: SerializeField]
    public float VoiceVolume { get; private set; } = 0.75f;
  }
}
