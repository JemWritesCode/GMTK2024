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

    [field: SerializeField]
    public Image BackgroundLogo { get; private set; }

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

    [field: Header("Powered")]
    [field: SerializeField]
    public Image PoweredIcon { get; private set; }

    [field: Header("Heat")]
    [field: SerializeField]
    public Image HeatIcon { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI HeatLabel { get; private set; }

    [field: Header("Fire")]
    [field: SerializeField]
    public Image FireIcon { get; private set; }

    [field: Header("Coolant")]
    [field: SerializeField]
    public Image CoolantIcon { get; private set; }

    [field: Header("Network")]
    [field: SerializeField]
    public Image NetworkIcon { get; private set; }

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
              .Insert(-0.1f, PanelRectTransform.DOPunchPosition(new(0f, -20f, 0f), 0.2f, 0, 0f))
              .Insert(0f, PanelCanvasGroup.DOFade(1f, 0.1f))
              .Insert(0f, BackgroundLogo.transform.DOLocalMove(new(0f, 5f, 0f), 0.2f).From(true))
              .Insert(0f, FadeMoveImage(UserIcon, new(0f, -3f, 0f), 0.2f))
              .Insert(0f, FadeMoveImage(UserLabel, new(-2f, 0f, 0f), 0.2f))
              .Insert(0f, FadeMoveImage(PowerIcon, new(0f, -3f, 0f), 0.2f))
              .Insert(0f, FadeMoveImage(PowerLabel, new(-2f, 0f, 0f), 0.2f))
              .Insert(0f, FadeMoveImage(PoweredIcon, new(0f, 3f, 0f), 0.2f))
              .Insert(0f, FadeMoveImage(HeatIcon, new(0f, -3f, 0f), 0.2f))
              .Insert(0f, FadeMoveImage(HeatLabel, new(-2f, 0f, 0f), 0.2f))
              .Insert(0f, FadeMoveImage(FireIcon, new(0f, 3f, 0f), 0.2f))
              .Insert(0f, FadeMoveImage(CoolantIcon, new(0f, 3f, 0f), 0.2f))
              .Insert(0f, FadeMoveImage(NetworkIcon, new(0f, 3f, 0f), 0.2f))
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

    static Tween FadeMoveImage(Graphic image, Vector3 offset, float duration) {
      return DOTween.Sequence()
          .Insert(0f, image.DOFade(1f, duration).From(0f, true))
          .Insert(0f, image.transform.DOLocalMove(offset, duration).From(true));
    }
  }
}
