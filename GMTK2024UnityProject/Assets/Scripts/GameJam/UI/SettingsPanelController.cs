using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class SettingsPanelController : MonoBehaviour {
    [field: Header("Panel")]
    [field: SerializeField]
    public RectTransform PanelRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup PanelCanvasGroup { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI PanelTitleLabel { get; private set; }

    [field: Header("OpenButton")]
    [field: SerializeField]
    public Image OpenButtonIcon { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI OpenButtonLabel { get; private set; }

    [field: Header("SizeDelta")]
    [field: SerializeField]
    public Vector2 ClosedSizeDelta { get; private set; }

    [field: SerializeField]
    public Vector2 OpenedSizeDelta { get; private set; }

    [field: Header("Settings")]
    [field: SerializeField]
    public SliderSettingController AudioVolumeSetting { get; private set; }

    [field: Header("State")]
    [field: SerializeField]
    public bool IsPanelOpen { get; private set; }

    private Sequence _openClosePanelTween;

    private void Start() {
      CreateTweens();
      ResetPanel();
    }

    private void CreateTweens() {
      _openClosePanelTween =
          DOTween.Sequence()
              .SetTarget(PanelRectTransform)
              .Insert(0f, PanelRectTransform.DOSizeDelta(OpenedSizeDelta, 0.5f).SetEase(Ease.InOutQuad))
              .Insert(0f, OpenButtonIcon.transform.DOBlendableRotateBy(new(0f, 0f, -180f), 0.5f).SetEase(Ease.Linear))
              .SetAutoKill(false)
              .Pause();
    }

    public void ResetPanel() {
      IsPanelOpen = false;
      PanelRectTransform.sizeDelta = ClosedSizeDelta;
      OpenButtonLabel.text = "Open";

      AudioVolumeSetting.SetValueWithoutNotify(AudioManager.SavedAudioVolume);
    }

    public void TogglePanel() {
      if (IsPanelOpen) {
        ClosePanel();
      } else {
        OpenPanel();
      }
    }

    public void OpenPanel() {
      IsPanelOpen = true;
      OpenButtonLabel.text = "Close";
      _openClosePanelTween.PlayAgain();
    }

    public void ClosePanel() {
      IsPanelOpen = false;
      OpenButtonLabel.text = "Open";
      _openClosePanelTween.SmoothRewind();
    }

    public void OnAudioVolumeValueChanged(float value) {
      AudioManager.Instance.SetAudioVolume(value);
    }
  }
}
