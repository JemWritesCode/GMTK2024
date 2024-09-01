using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class InteractPanelController : MonoBehaviour {
    [field: Header("Panel")]
    [field: SerializeField]
    public RectTransform PanelRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup PanelCanvasGroup { get; private set; }

    [field: Header("Interact")]
    [field: SerializeField]
    public TextMeshProUGUI InteractText { get; private set; }

    [field: SerializeField]
    public Image InteractIcon { get; private set; }

    public bool IsPanelVisible { get; private set; }
    public Interactable CurrentInteractable { get; private set; }

    private Sequence _showHidePanelTween;

    private void Start() {
      CreateTweens();
      ResetPanel();
    }

    private void CreateTweens() {
      _showHidePanelTween =
          DOTween.Sequence()
              .SetTarget(gameObject)
              .Insert(-0.1f, PanelRectTransform.DOPunchPosition(new(20f, 0f, 0f), 0.2f, 0, 0f).SetEase(Ease.OutQuad))
              .Insert(0f, PanelCanvasGroup.DOFade(1f, 0.1f).SetEase(Ease.InOutQuad))
              .Insert(0f, InteractIcon.transform.DOBlendableLocalMoveBy(new(0f, -5f, 0f), 0.2f).From(isRelative: true))
              .Insert(0f, InteractText.transform.DOBlendableLocalMoveBy(new(10f, 0f, 0f), 0.2f).From(isRelative: true))
              .SetAutoKill(false)
              .Pause();
    }

    public void ResetPanel() {
      PanelCanvasGroup.alpha = 0f;
      PanelCanvasGroup.blocksRaycasts = false;
      IsPanelVisible = false;

      InteractText.text = "...";
    }

    public void ShowPanel(string interactText) {
      InteractText.text = interactText;

      PanelCanvasGroup.blocksRaycasts = true;
      IsPanelVisible = true;

      _showHidePanelTween.PlayAgain();
    }

    public void HidePanel() {
      PanelCanvasGroup.blocksRaycasts = false;
      IsPanelVisible = false;

      _showHidePanelTween.SmoothRewind();
    }

    public void SetInteractable(Interactable interactable) {
      if (CurrentInteractable == interactable) {
        return;
      }

      CurrentInteractable = interactable;

      if (interactable) {
        ShowPanel(interactable.InteractText);
      } else {
        HidePanel();
      }
    }
  }
}
