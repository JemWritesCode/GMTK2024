using Coffee.UIEffects;

using DG.Tweening;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class StartScreenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [field: Header("UI")]
    [field: SerializeField]
    public Image Image { get; private set; }

    [field: SerializeField]
    public UIGradient ImageGradient { get; private set; }

    [field: Header("OnHover")]
    [field: SerializeField]
    public float TweenToDuration { get; private set; }

    [field: SerializeField]
    public Vector3 TweenToScale { get; private set; }

    [field: SerializeField]
    public Color TweenToColor { get; private set; }

    [field: SerializeField]
    public Vector3 TweenToRotation { get; private set; }

    [field: Header("SFX")]
    [field: SerializeField]
    public AudioSource SfxAudioSource { get; private set; }

    [field: SerializeField]
    public AudioClip OnHoverSfx { get; private set; }

    private Sequence _onHoverTween;
    private Sequence _playSfxTween;

    private void Start() {
      _onHoverTween =
          DOTween.Sequence()
              .SetLink(gameObject)
              .Insert(0f, transform.DOScale(TweenToScale, TweenToDuration))
              .Insert(0f, ImageGradient.DOBlendableColor2(TweenToColor, TweenToDuration))
              .Insert(0f, Image.transform.DOLocalMove(new(0f, -5f, 0f), TweenToDuration).From(isRelative: true))
              .Insert(0f, Image.transform.DOBlendableLocalRotateBy(TweenToRotation, TweenToDuration))
              .SetAutoKill(false)
              .SetUpdate(true)
              .Pause();

      if (SfxAudioSource && OnHoverSfx) {
        float duration = OnHoverSfx.length;

        _playSfxTween =
            DOTween.Sequence()
                .SetLink(gameObject)
                .InsertCallback(0f, () => SfxAudioSource.PlayOneShot(OnHoverSfx, 0.5f))
                .AppendInterval(duration)
                .SetAutoKill(false)
                .SetUpdate(true)
                .Pause();
      }
    }

    public void OnPointerEnter(PointerEventData eventData) {
      _onHoverTween.PlayAgain();

      if (_playSfxTween != null && !_playSfxTween.IsPlaying()) { 
        _playSfxTween.Restart();
      }
    }

    public void OnPointerExit(PointerEventData eventData) {
      _onHoverTween.SmoothRewind();
    }
  }
}
