using DG.Tweening;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class StartScreenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [field: SerializeField]
    public Image Image { get; private set; }

    [field: Header("OnHover")]
    [field: SerializeField]
    public float TweenToDuration { get; private set; }

    [field: SerializeField]
    public Vector3 TweenToScale { get; private set; }

    [field: SerializeField]
    public Color TweenToColor { get; private set; }

    private Sequence _onHoverTween;

    private void Start() {
      _onHoverTween =
          DOTween.Sequence()
              .SetLink(gameObject)
              .Insert(0f, transform.DOScale(TweenToScale, TweenToDuration))
              .Insert(0f, Image.DOColor(TweenToColor, TweenToDuration))
              .SetEase(Ease.InOutQuad)
              .SetAutoKill(false)
              .SetUpdate(true)
              .Pause();
    }

    public void OnPointerEnter(PointerEventData eventData) {
      _onHoverTween.PlayAgain();
    }

    public void OnPointerExit(PointerEventData eventData) {
      _onHoverTween.SmoothRewind();
    }
  }
}
