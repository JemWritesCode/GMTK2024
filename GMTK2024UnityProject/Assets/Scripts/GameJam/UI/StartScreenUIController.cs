using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace GameJam {
  public sealed class StartScreenUIController : MonoBehaviour {
    [field: SerializeField]
    public Image GameLogo { get; private set; }

    private void Awake() {
      AnimateIntro();
    }

    public void AnimateIntro() {
      DOTween.Sequence()
          .SetTarget(gameObject)
          .Insert(0f, CreateTranslateFade(GameLogo, 0.5f, new(0f, -150f, 0f), 2f))
          .SetEase(Ease.InOutQuad);
    }

    private Sequence CreateTranslateFade(Image image, float atPosition, Vector3 translateDirection, float duration) {
      return DOTween.Sequence()
          .Insert(
              atPosition - (duration / 2f),
              image.transform.DOPunchPosition(Vector3.Scale(translateDirection, Vector3.one * -1f), duration, 0, 0f))
          .Insert(
              atPosition,
              image
                  .DOFade(1f, duration / 2f)
                  .From(0f));
    }
  }
}
