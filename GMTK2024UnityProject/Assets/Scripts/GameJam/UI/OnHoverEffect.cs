using DG.Tweening;

using UnityEngine;
using UnityEngine.EventSystems;

using YoloBox;

namespace GameJam {
  public sealed class OnHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [field: SerializeField]
    public Vector3 TweenScaleTo { get; private set; } = Vector3.one;

    [field: SerializeField, Min(0f)]
    public float TweenScaleDuration { get; private set; } = 0.5f;

    Tweener _onHoverTweener;

    void Start() {
      _onHoverTweener =
          transform
              .DOScale(TweenScaleTo, TweenScaleDuration)
              .SetEase(Ease.InOutQuad)
              .SetLink(gameObject)
              .SetAutoKill(false)
              .SetUpdate(true)
              .Pause();
    }

    public void OnPointerEnter(PointerEventData eventData) {
      _onHoverTweener.PlayAgain();
    }

    public void OnPointerExit(PointerEventData eventData) {
      _onHoverTweener.SmoothRewind();
    }
  }
}
