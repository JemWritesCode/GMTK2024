using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class UserCountPanelController : MonoBehaviour {
    [field: Header("Panel")]
    [field: SerializeField]
    public RectTransform PanelRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup PanelCanvasGroup { get; private set; }

    [field: Header("UserCount")]
    [field: SerializeField]
    public Image UserCountIcon { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI UserCountLabel { get; private set; }

    [field: Header("State")]
    [field: SerializeField]
    public bool IsPanelVisible { get; private set; }

    [field: SerializeField]
    public int CurrentUserCountValue { get; private set; }

    Sequence _showHidePanelTween;

    private void Start() {
      CreateTweens();
      ResetPanel();
    }

    private void CreateTweens() {
      _showHidePanelTween =
          DOTween.Sequence()
              .SetTarget(PanelRectTransform)
              .Insert(0f, PanelCanvasGroup.DOFade(1f, 0.5f))
              .Insert(0f, PanelRectTransform.DOLocalMove(new(0f, 15f, 0f), 0.5f).From(false, true))
              .SetAutoKill(false)
              .Pause();
    }

    public void ResetPanel() {
      PanelCanvasGroup.alpha = 0f;
      PanelCanvasGroup.blocksRaycasts = false;
      IsPanelVisible = false;

      UserCountLabel.text = "0";
      CurrentUserCountValue = 0;
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
    }

    public void HidePanel() {
      PanelCanvasGroup.blocksRaycasts = false;
      IsPanelVisible = false;

      _showHidePanelTween.SmoothRewind();
    }

    public void SetUserCountValue(int userCountValue) {
      UserCountLabel.DOComplete(withCallbacks: true);

      DOTween.Sequence()
          .SetTarget(UserCountLabel)
          .Insert(0f, UserCountLabel.DOCounter(CurrentUserCountValue, userCountValue, 0.5f))
          .Insert(0f, UserCountLabel.transform.DOPunchPosition(new(0f, 1f, 0f), 1f, 3, 1f))
          .Insert(0f, UserCountIcon.transform.DOPunchScale(Vector3.one * 0.1f, 1f, 5, 0f));

      CurrentUserCountValue = userCountValue;
    }
  }
}
