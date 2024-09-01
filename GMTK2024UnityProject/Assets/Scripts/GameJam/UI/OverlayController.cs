using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class OverlayController : MonoBehaviour {
    [field: Header("Overlay")]
    [field: SerializeField]
    public RectTransform OverlayRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup OverlayCanvasGroup { get; private set; }

    [field: SerializeField]
    public Image LogoImage { get; private set; }

    [field: Header("State")]
    [field: SerializeField]
    public bool IsOverlayVisible { get; private set; }

    private Sequence _showHideOverlayTween;

    private void Start() {
      CreateTweens();
      ResetOverlay();
    }

    private void CreateTweens() {
      _showHideOverlayTween =
          DOTween.Sequence()
              .SetTarget(OverlayRectTransform)
              .Insert(0f, OverlayCanvasGroup.DOFade(1f, 0.5f))
              .Insert(0f, LogoImage.transform.DOBlendableMoveBy(new(-50f, 0f, 0f), 0.5f).From(false, true))
              .Insert(0f, LogoImage.transform.DOBlendableScaleBy(Vector3.one * 0.05f, 0.5f))
              .SetAutoKill(false)
              .SetUpdate(isIndependentUpdate: true)
              .Pause();
    }

    public void ResetOverlay() {
      OverlayCanvasGroup.alpha = 0f;
      OverlayCanvasGroup.blocksRaycasts = false;
      IsOverlayVisible = false;
    }

    public void ToggleOverlay(bool toggleOn) {
      IsOverlayVisible = toggleOn;
      _showHideOverlayTween.PlayOrRewind(toggleOn);
    }
  }
}
