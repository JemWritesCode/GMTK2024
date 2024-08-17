using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class ServerPanelController : MonoBehaviour {
    [field: Header("Panel")]
    [field: SerializeField]
    public RectTransform PanelRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup PanelCanvasGroup { get; private set; }

    [field: Header("User")]
    [field: SerializeField]
    public Image UserIcon { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI UserLabel { get; private set; }

    [field: Header("Power")]
    [field: SerializeField]
    public Image PowerIcon { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI PowerLabel { get; private set; }

    [field: Header("State")]
    [field: SerializeField]
    public bool IsPanelVisible { get; private set; }

    private Sequence _showHidePanelTween;

    private void Start() {
      CreateTweens();
      ResetPanel();
    }

    private void CreateTweens() {
      _showHidePanelTween =
          DOTween.Sequence()
              .SetTarget(gameObject)
              .Insert(-0.1f, PanelRectTransform.DOPunchPosition(new(20f, 0f, 0f), 0.2f, 0, 0f))
              .Insert(0f, PanelCanvasGroup.DOFade(1f, 0.1f))
              .SetAutoKill(false)
              .Pause();
    }

    public void ResetPanel() {
      PanelCanvasGroup.alpha = 0f;
      PanelCanvasGroup.blocksRaycasts = false;
      IsPanelVisible = false;
    }

    public void ShowPanel() {
      PanelCanvasGroup.blocksRaycasts = true;
      IsPanelVisible = true;

      _showHidePanelTween.PlayAgain();
    }

    public void HidePanel() {
      PanelCanvasGroup.blocksRaycasts = false;
      IsPanelVisible = false;

      _showHidePanelTween.SmoothRewind();
    }
  }
}
