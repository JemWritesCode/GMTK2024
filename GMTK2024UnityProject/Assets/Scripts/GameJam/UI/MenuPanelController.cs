using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class MenuPanelController : MonoBehaviour {
    [field: Header("Panel")]
    [field: SerializeField]
    public RectTransform PanelRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup PanelCanvasGroup { get; private set; }

    [field: Header("Buttons")]
    [field: SerializeField]
    public Button CloseButton { get; private set; }

    [field: Header("Settings")]
    [field: SerializeField]
    public Slider AudioVolumeSlider { get; private set; }

    [field: SerializeField]
    public Slider DialogVolumeSlider { get; private set; }

    [field: Header("State")]
    [field: SerializeField]
    public bool IsPanelVisible { get; private set; }

    Sequence _showHidePanelTween;

    private void Start() {
      ResetPanel();

      _showHidePanelTween =
          DOTween.Sequence()
              .SetTarget(PanelRectTransform)
              .Insert(0f, PanelCanvasGroup.DOFade(1f, 0.5f))
              .Insert(0f, PanelRectTransform.DOLocalMove(new(-25f, 0f, 0f), 0.5f).From(false, true))
              .SetEase(Ease.InOutQuad)
              .SetAutoKill(false)
              .Pause();
    }

    public void ResetPanel() {
      PanelCanvasGroup.alpha = 0f;
      PanelCanvasGroup.blocksRaycasts = false;

      IsPanelVisible = false;
    }

    public void TogglePanel() {
      if (IsPanelVisible) {
        HidePanel();
      } else {
        ShowPanel();
      }
    }

    public void ShowPanel() {
      PanelCanvasGroup.blocksRaycasts = true;
      IsPanelVisible = true;

      AudioVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.AudioVolume);
      DialogVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.DialogVolume);

      _showHidePanelTween.PlayAgain();
    }

    public void HidePanel() {
      PanelCanvasGroup.blocksRaycasts = false;
      IsPanelVisible = false;

      _showHidePanelTween.SmoothRewind();
    }

    public void OnAudioVolumeValueChanged(float value) {
      AudioManager.Instance.SetAudioVolume(value);
    }

    public void OnDialogVolumeValueChanged(float value) {
      AudioManager.Instance.SetDialogVolume(value);
    }
  }
}
