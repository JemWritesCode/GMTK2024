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

    [field: SerializeField]
    public Server CurrentServer { get; private set; }

    [field: SerializeField]
    public int CurrentUserValue { get; private set; }

    [field: SerializeField]
    public int CurrentPowerValue { get; private set; }

    [field: SerializeField]
    public float CurrentHeatValue { get; private set; }

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

    static Sequence FadeMoveImage(Graphic image, Vector3 offset, float duration) {
      return DOTween.Sequence()
          .Insert(0f, image.DOFade(1f, duration).From(0f, true))
          .Insert(0f, image.transform.DOLocalMove(offset, duration).From(true));
    }

    public void ResetPanel() {
      PanelCanvasGroup.alpha = 0f;
      PanelCanvasGroup.blocksRaycasts = false;
      IsPanelVisible = false;

      CurrentServer = default;

      UserLabel.text = "0";
      CurrentUserValue = 0;

      PowerLabel.text = "0";
      CurrentPowerValue = 0;

      HeatLabel.text = "0%";
      CurrentHeatValue = 0f;
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

    public void SetServer(Server server) {
      if (CurrentServer == server) {
        return;
      }

      CurrentServer = server;

      if (server) {
        RefreshPanel(server);
        ShowPanel();
      } else {
        HidePanel();
      }
    }

    private void RefreshPanel(Server server) {
      SetUserValue(server.UserCapacity);
      SetPowerValue(server.RequiredPower);
      SetHeatValue(server.Temperature.HeatPercent());
    }

    public void SetUserValue(int userValue) {
      UserLabel.DOComplete(withCallbacks: true);

      DOTween.Sequence()
          .SetTarget(UserLabel)
          .Insert(0f, UserLabel.DOCounter(CurrentPowerValue, userValue, 0.5f))
          .Insert(0f, UserLabel.transform.DOPunchPosition(new(0f, 1f, 0f), 1f, 3, 1f))
          .Insert(0f, UserIcon.transform.DOPunchScale(Vector3.one * 0.1f, 1f, 5, 0f));

      CurrentUserValue = userValue;
    }

    public void SetPowerValue(int powerValue) {
      PowerLabel.DOComplete(withCallbacks: true);

      DOTween.Sequence()
          .SetTarget(PowerLabel)
          .Insert(0f, PowerLabel.DOCounter(CurrentPowerValue, powerValue, 0.5f))
          .Insert(0f, PowerLabel.transform.DOPunchPosition(new(0f, 1f, 0f), 1f, 3, 1f))
          .Insert(0f, PowerIcon.transform.DOPunchScale(Vector3.one * 0.1f, 1f, 5, 0f));

      CurrentPowerValue = powerValue;
    }

    public void SetHeatValue(float heatValue) {
      HeatLabel.DOComplete(withCallbacks: true);

      DOTween.Sequence()
          .SetTarget(HeatLabel)
          .Insert(0f, HeatLabel.DOPercentCounter(CurrentHeatValue, heatValue, 0.5f))
          .Insert(0f, HeatLabel.transform.DOPunchPosition(new(0f, 1f, 0f), 1f, 3, 1f))
          .Insert(0f, HeatIcon.transform.DOPunchScale(Vector3.one * 0.1f, 1f, 5, 0f));

      CurrentHeatValue = heatValue;
    }
  }
}
