using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;

using YoloBox;

namespace GameJam {
  public sealed class MenuPanelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [field: Header("UI")]
    [field: SerializeField]
    public Image Image { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI Label { get; private set; }

    [field: SerializeField]
    public Image Icon { get; private set; }

    [field: Header("OnHover")]
    [field: SerializeField]
    public float TweenToDuration { get; private set; }

    [field: SerializeField]
    public Vector3 TweenToScale { get; private set; }

    [field: SerializeField]
    public Vector3 TweenToRotation { get; private set; }

    [field: SerializeField]
    public Color TweenToColor { get; private set; }

    private Sequence _onHoverTween;

    private void Start() {
      _onHoverTween =
          DOTween.Sequence()
              .SetLink(gameObject)
              .Insert(0f, Label.transform.DOScale(TweenToScale, TweenToDuration))
              .Insert(0f, Image.DOColor(TweenToColor, TweenToDuration))
              .Insert(0f, Icon.transform.DOScale(TweenToScale, TweenToDuration))
              .Insert(0f, Icon.transform.DOLocalRotate(TweenToRotation, TweenToDuration))
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
