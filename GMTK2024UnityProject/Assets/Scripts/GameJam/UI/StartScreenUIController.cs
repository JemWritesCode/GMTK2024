using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace GameJam {
  public sealed class StartScreenUIController : MonoBehaviour {
    [field: SerializeField]
    public Image GameLogo { get; private set; }

    private void Awake() {
      CreateTweens();
      AnimateIntro();
    }

    public void AnimateIntro() {
      DOTween.Sequence()
          .SetTarget(gameObject)
          .Insert(0f, CreateTranslateFade(GameLogo, 0.25f, new(0f, -200f, 0f), 0f, 3f))
          .SetEase(Ease.InOutQuad);
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

    private Sequence _gameLogoOnClickTween;

    private void CreateTweens() {
      _gameLogoOnClickTween =
          DOTween.Sequence()
              .SetAutoKill(false)
              .Insert(0f, GameLogo.transform.DOPunchScale(Vector3.one * 0.05f, 0.5f, 10, 1f))
              .Pause();
    }

    public void OnGameLogoClick() {
      _gameLogoOnClickTween.Complete();
      _gameLogoOnClickTween.Restart();
    }
  }
}
