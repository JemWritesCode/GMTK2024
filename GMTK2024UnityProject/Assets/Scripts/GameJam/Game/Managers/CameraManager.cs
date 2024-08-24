using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class CameraManager : SingletonManager<CameraManager> {
    [field: Header("Camera")]
    [field: SerializeField]
    public Camera GameCamera { get; private set; }

    [field: Header("Settings")]
    [field: SerializeField]
    public float FieldOfViewHorizontal { get; private set; }

    public static float SavedFieldOfViewHorizontal { get; private set; } = 0f;

    private void Start() {
      SetFieldOfViewHorizontal(SavedFieldOfViewHorizontal > 0f ? SavedFieldOfViewHorizontal : FieldOfViewHorizontal);
    }

    public void SetFieldOfViewHorizontal(float value) {
      SavedFieldOfViewHorizontal = Mathf.Clamp(value, 45f, 105f);
      FieldOfViewHorizontal = SavedFieldOfViewHorizontal;

      GameCamera.fieldOfView = Camera.HorizontalToVerticalFieldOfView(value, GameCamera.aspect);
    }
  }
}
