using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class DialogPanelController : MonoBehaviour {
    [field: Header("Panel")]
    [field: SerializeField]
    public RectTransform PanelRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup PanelCanvasGroup { get; private set; }

    [field: Header("Dialog")]
    [field: SerializeField]
    public Image PortraitImage { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI DialogText { get; private set; }

    [field: Header("Buttons")]
    [field: SerializeField]
    public Button ConfirmButton { get; private set; }

    [field: Header("SFX")]
    [field: SerializeField]
    public AudioSource SfxAudioSource { get; private set; }

    [field: SerializeField]
    public AudioClip ShowPanelSfx { get; private set; }

    [field: Header("State")]
    [field: SerializeField]
    public bool IsPanelVisible { get; private set; }

    Sequence _showHidePanelTween;

    private void Awake() {
      ResetPanel();

      _showHidePanelTween =
          DOTween.Sequence()
              .SetTarget(PanelRectTransform)
              .Insert(0f, PanelCanvasGroup.DOFade(1f, 0.4f))
              .Insert(0f, PanelRectTransform.DOLocalMove(new(0f, 25f, 0f), 0.5f).From(false, true))
              .Insert(0f, PortraitImage.transform.DOPunchScale(Vector3.one * 0.05f, 0.5f, 0, 0f))
              .Insert(0f, DialogText.transform.DOLocalMoveY(5f, 0.5f).From(true))
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

      _showHidePanelTween.PlayAgain();
      SfxAudioSource.PlayOneShot(ShowPanelSfx);
    }

    public void HidePanel() {
      PanelCanvasGroup.blocksRaycasts = false;
      IsPanelVisible = false;

      _showHidePanelTween.SmoothRewind();
    }
  }
}
