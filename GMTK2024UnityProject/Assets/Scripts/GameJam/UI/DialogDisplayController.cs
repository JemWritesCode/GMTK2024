using DG.Tweening;

using DS.ScriptableObjects;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class DialogDisplayController : MonoBehaviour {
    [field: SerializeField]
    public RectTransform DisplayRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup DisplayCanvasGroup { get; private set; }

    [field: SerializeField]
    public Image PortraitImage { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI DialogText { get; private set; }

    private Sequence _showDisplayTween;
    private Sequence _portraitImageClickTween;

    private void Start() {
      CreateTweens();
    }

    private void CreateTweens() {
      _showDisplayTween =
          DOTween.Sequence()
              .SetTarget(DisplayRectTransform)
              .Insert(0f, DisplayCanvasGroup.DOFade(1f, 0.2f))
              .Insert(0f, PortraitImage.transform.DOPunchScale(Vector3.one * 0.05f, 0.4f, 0, 0f))
              .Insert(0f, DialogText.transform.DOLocalMoveY(5f, 0.4f).From(true))
              .SetAutoKill(false)
              .Pause();

      _portraitImageClickTween =
          DOTween.Sequence()
              .SetTarget(PortraitImage)
              .Insert(0f, PortraitImage.transform.DOPunchScale(Vector3.one * 0.05f, 0.5f, 10, 1f))
              .SetAutoKill(false)
              .Pause();
    }

    public void ToggleDisplay(bool toggleOn) {
      DisplayCanvasGroup.blocksRaycasts = toggleOn;
      _showDisplayTween.PlayOrRewind(toggleOn);
    }

    public void SetupDisplay(DSDialogueSO dialogNode) {
      if (dialogNode.Portrait) {
        PortraitImage.sprite = dialogNode.Portrait;
      }

      DialogText.text = dialogNode.Text;
    }

    public void OnPortraitImageClick() {
      _portraitImageClickTween.PlayComplete();
    }
  }
}
