using DG.Tweening;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class MenuOverlayController : MonoBehaviour {
    [field: Header("Overlay")]
    [field: SerializeField]
    public RectTransform OverlayRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup OverlayCanvasGroup { get; private set; }

    [field: Header("Panels")]
    [field: SerializeField]
    public MenuPanelController MenuPanel { get; private set; }

    [field: SerializeField]
    public SettingsPanelController SettingsPanel { get; private set; }

    [field: Header("State")]
    [field: SerializeField]
    public bool IsOverlayVisible { get; private set; }

    private Sequence _showHideOverlayTween;

    private void Start() {
      MenuPanel.CloseButton.OnPointerDownClick.AddListener(HideOverlay);

      CreateTweens();
      ResetOverlay();
    }

    private void CreateTweens() {
      _showHideOverlayTween =
          DOTween.Sequence()
              .SetTarget(OverlayRectTransform)
              .Insert(0f, OverlayCanvasGroup.DOFade(1f, 0.5f))
              .SetAutoKill(false)
              .SetUpdate(isIndependentUpdate: true)
              .Pause();
    }

    public void ResetOverlay() {
      OverlayCanvasGroup.alpha = 0f;
      OverlayCanvasGroup.blocksRaycasts = false;
      IsOverlayVisible = false;
    }

    public void ToggleOverlay() {
      if (IsOverlayVisible) {
        HideOverlay();
      } else {
        ShowOverlay();
      }
    }

    public void ShowOverlay() {
      OverlayCanvasGroup.blocksRaycasts = true;
      IsOverlayVisible = true;

      _showHideOverlayTween.PlayAgain();
      MenuPanel.ShowPanel();
      SettingsPanel.ShowPanel();
    }

    public void HideOverlay() {
      OverlayCanvasGroup.blocksRaycasts = false;
      IsOverlayVisible = false;

      SettingsPanel.HidePanel();
      MenuPanel.HidePanel();
      _showHideOverlayTween.SmoothRewind();
    }

    public void QuitGame() {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
  }
}
