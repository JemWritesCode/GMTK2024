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
    public Image GameLogoImage { get; private set; }

    [field: SerializeField]
    public Image StartButtonImage { get; private set; }

    [field: SerializeField]
    public Image SettingsButtonImage { get; private set; }

    [field: SerializeField]
    public Image CreditsButtonImage { get; private set; }

    [field: Header("Scene")]
    [field: SerializeField]
    public SceneReference GameScene { get; private set; }

    private void Awake() {
      CreateTweens();
      AnimateIntro();
    }

    public void AnimateIntro() {
      DOTween.Complete(gameObject, withCallbacks: true);

      DOTween.Sequence()
          .SetTarget(gameObject)
          .Insert(0f, CreateTranslateFade(GameLogoImage, 0.25f, new(0f, -200f, 0f), 0f, 2.5f))
          .Insert(0.5f, CreateTranslateFade(StartButtonImage, 0.25f, new(0f, 50f, 0f), 0f, 3f))
          .Insert(0.5f, CreateTranslateFade(SettingsButtonImage, 0.75f, new(-50f, 0f, 0f), 0f, 3f))
          .Insert(0.5f, CreateTranslateFade(CreditsButtonImage, 1.25f, new(-50f, 0f, 0f), 0f, 3f))
          .SetEase(Ease.InOutQuad);
    }

    private Sequence _gameLogoOnClickTween;
    private Sequence _loadSceneTween;

    private void CreateTweens() {
      _gameLogoOnClickTween =
          DOTween.Sequence()
              .SetAutoKill(false)
              .Insert(0f, GameLogoImage.transform.DOPunchScale(Vector3.one * 0.05f, 0.5f, 10, 1f))
              .Pause();
    }

    private Sequence CreateTranslateFade(
        Image image, float atPosition, Vector3 translateDirection, float fadeFrom, float duration) {
      return DOTween.Sequence()
          .Insert(
              atPosition - (duration / 2f),
              image.transform.DOPunchPosition(Vector3.Scale(translateDirection, Vector3.one * -1f), duration, 0, 0f))
          .Insert(
              atPosition,
              image
                  .DOFade(1f, duration / 2f)
                  .From(fadeFrom));
    }

    public void OnGameLogoClick() {
      _gameLogoOnClickTween.PlayAgain();
    }

    public void OnStartButtonClick() {
      LoadScene(GameScene);
    }

    private void LoadScene(SceneReference scene) {
      if (!_loadSceneTween.IsActive()) {
        _loadSceneTween =
            DOTween.Sequence()
                .InsertCallback(0.5f, () => SceneManager.LoadScene(scene.Path));
      }
    }
  }
}
