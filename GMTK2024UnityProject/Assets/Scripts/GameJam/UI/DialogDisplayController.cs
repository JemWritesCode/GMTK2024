using DG.Tweening;

using DS.Data;
using DS.Enumerations;
using DS.ScriptableObjects;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class DialogDisplayController : MonoBehaviour {
    [field: Header("UI")]
    [field: SerializeField]
    public RectTransform DisplayRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup DisplayCanvasGroup { get; private set; }

    [field: SerializeField]
    public Image Background { get; private set; }

    [field: SerializeField]
    public Image PortraitImage { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI DialogText { get; private set; }

    [field: Header("Confirm")]
    [field: SerializeField]
    public Button ConfirmButton { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI ConfirmLabel { get; private set; }

    [field: Header("Cancel")]
    [field: SerializeField]
    public Button CancelButton { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI CancelLabel { get; private set; }

    [field: Header("Type")]
    [field: SerializeField]
    public DSPortraitType DisplayPortraitType { get; private set; }

    private Sequence _showDisplayTween;
    private Sequence _portraitImageClickTween;

    private void Start() {
      CreateTweens();
    }

    private void CreateTweens() {
      _showDisplayTween =
          DOTween.Sequence()
              .SetTarget(DisplayRectTransform)
              .Insert(0f, DisplayCanvasGroup.DOFade(1f, 0.4f))
              .Insert(0f, PortraitImage.transform.DOPunchScale(Vector3.one * 0.05f, 0.4f, 0, 0f))
              .Insert(0f, DialogText.transform.DOBlendableLocalMoveBy(new(0f, 5f, 0f), 0.4f).From(true))
              .SetAutoKill(false)
              .SetUpdate(isIndependentUpdate: true)
              .Pause();

      _portraitImageClickTween =
          DOTween.Sequence()
              .SetTarget(PortraitImage)
              .Insert(0f, PortraitImage.transform.DOPunchScale(Vector3.one * 0.05f, 0.5f, 10, 1f))
              .SetAutoKill(false)
              .SetUpdate(isIndependentUpdate: true)
              .Pause();
    }

    public void ShowDisplay() {
      DisplayCanvasGroup.blocksRaycasts = true;
      _showDisplayTween.PlayAgain();
    }

    public void HideDisplay() {
      DisplayCanvasGroup.blocksRaycasts = false;
      _showDisplayTween.SmoothRewind();
    }

    public void SetupDisplay(DSDialogueSO dialogNode) {
      if (dialogNode.Portrait) {
        PortraitImage.sprite = dialogNode.Portrait;
      }

      DialogText.text = dialogNode.Text;

      SetupConfirmButton(dialogNode);
      SetupCancelButton(dialogNode);
    }

    private void SetupConfirmButton(DSDialogueSO dialogNode) {
      ConfirmLabel.text =
          dialogNode.Choices.Count > 0 && dialogNode.Choices[0].HasCustomChoiceText()
              ? dialogNode.Choices[0].Text
              : "Confirm";
    }

    private void SetupCancelButton(DSDialogueSO dialogNode) {
      if (CancelButton) {
        CancelButton.gameObject.SetActive(dialogNode.Choices.Count > 1);

        CancelLabel.text =
            dialogNode.Choices.Count > 1 && dialogNode.Choices[1].HasCustomChoiceText()
                ? dialogNode.Choices[1].Text
                : "Cancel";
      }
    }

    public void OnPortraitImageClick() {
      _portraitImageClickTween.PlayComplete();
    }
  }
}
