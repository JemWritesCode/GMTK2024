using DG.Tweening;

using Eflatun.SceneReference;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class StartScreenUIController : MonoBehaviour {
    [field: Header("UI")]
    [field: SerializeField]
    public RectTransform LogoRectTransform { get; private set; }

    [field: SerializeField]
    public Image LogoBow { get; private set; }

    [field: SerializeField]
    public Image StartButtonImage { get; private set; }

    [field: SerializeField]
    public Image SettingsButtonImage { get; private set; }

    [field: SerializeField]
    public Image CreditsButtonImage { get; private set; }

    [field: SerializeField]
    public Image LoadingOverlayImage { get; private set; }

    [field: Header("Scene")]
    [field: SerializeField]
    public SceneReference GameScene { get; private set; }

    private void Start() {
      CreateTweens();
      AnimateIntro();
    }

    public void AnimateIntro() {
      DOTween.Complete(gameObject, withCallbacks: true);

      DOTween.Sequence()
          .SetTarget(gameObject)
          .SetLink(gameObject)
          .Insert(0f, LoadingOverlayImage.DOColor(Color.black, 0f))
          .Insert(0.5f, LoadingOverlayImage.DOFade(0f, 3f).From(1f, true).SetEase(Ease.InSine))
          .Insert(0.5f, LogoRectTransform.DOLocalMove(new(0f, -50f, 0f), 2f).From(true))
          .Insert(0.5f, StartButtonImage.rectTransform.DOLocalMove(new(0f, 20f, 0f), 1f).From(true));
    }

    private Sequence _gameLogoOnClickTween;
    private Sequence _loadSceneTween;

    private void CreateTweens() {
      _gameLogoOnClickTween =
          DOTween.Sequence()
              .SetAutoKill(false)
              .SetTarget(LogoBow)
              .SetLink(LogoBow.gameObject)
              .Insert(0f, LogoBow.rectTransform.DOPunchScale(Vector3.one * 0.05f, 0.5f, 10, 1f))
              .Pause();
    }

    public void OnGameLogoClick() {
      _gameLogoOnClickTween.PlayComplete();
    }

    public void OnStartButtonClick() {
      Cursor.lockState = CursorLockMode.Locked;
      LoadScene(GameScene);
    }

    private void LoadScene(SceneReference scene) {
      if (!_loadSceneTween.IsActive()) {
        _loadSceneTween =
            DOTween.Sequence()
                .Insert(0f, LoadingOverlayImage.DOFade(1f, 2f))
                .AppendInterval(1f)
                .AppendCallback(() => SceneManager.LoadScene(scene.Path));
      }
    }
  }
}
