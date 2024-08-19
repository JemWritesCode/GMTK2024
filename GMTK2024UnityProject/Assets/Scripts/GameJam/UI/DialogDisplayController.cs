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

    private void Start() {
      CreateTweens();
    }

    private void CreateTweens() {
      _showDisplayTween =
          DOTween.Sequence()
              .SetTarget(DisplayRectTransform)
              .Insert(0f, DisplayCanvasGroup.DOFade(1f, 0.3f))
              .Insert(0f, PortraitImage.transform.DOPunchScale(Vector3.one * 0.05f, 0.4f, 0, 0f))
              .Insert(0f, DialogText.transform.DOLocalMoveY(5f, 0.4f).From(true))
              .SetAutoKill(false)
              .Pause();
    }

    public void ToggleDisplay(bool toggleOn) {
      _showDisplayTween.PlayOrRewind(toggleOn);
    }

    public void SetupDisplay(DSDialogueSO dialogNode) {
      if (dialogNode.Portrait) {
        PortraitImage.sprite = dialogNode.Portrait;
      }

      DialogText.text = dialogNode.Text;
    }
  }
}
