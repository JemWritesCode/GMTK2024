using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class AudioManager : SingletonManager<AudioManager> {
    [field: Header("Volumes")]
    [field: SerializeField]
    public float AudioVolume { get; private set; } = 0.75f;

    [field: SerializeField]
    public float DialogVolume { get; private set; } = 0.75f;

    [field: Header("AudioSources")]
    [field: SerializeField]
    public AudioSource DialogAudioSource { get; private set; }

    private void Start() {
      SetAudioVolume(AudioVolume);
      SetDialogVolume(DialogVolume);
    }

    public void SetAudioVolume(float audioVolume) {
      AudioVolume = Mathf.Clamp01(audioVolume);
      AudioListener.volume = AudioVolume;
    }

    public void SetDialogVolume(float dialogVolume) {
      DialogVolume = Mathf.Clamp01(dialogVolume);
      DialogAudioSource.volume = Mathf.Min(dialogVolume, DialogAudioSource.volume);
    }
  }
}
