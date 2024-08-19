using System.Linq;

using DG.Tweening;

using DS.ScriptableObjects;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class DialogPanelController : MonoBehaviour {
    [field: Header("Panel")]
    [field: SerializeField]
    public RectTransform PanelRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup PanelCanvasGroup { get; private set; }

    [field: Header("Dialog")]
    [field: SerializeField]
    public GameObject Portrait { get; private set; }

    [field: SerializeField]
    public Image PortraitImage { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI DialogText { get; private set; }

    [field: Header("TopPortrait")]
    [field: SerializeField]
    public GameObject TopPortrait { get; private set; }

    [field: SerializeField]
    public Image TopPortraitImage { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI TopPortraitText { get; private set; }

    [field: Header("Buttons")]
    [field: SerializeField]
    public Button ConfirmButton { get; private set; }

    [field: Header("SFX")]
    [field: SerializeField]
    public AudioSource SfxAudioSource { get; private set; }

    [field: SerializeField]
    public AudioClip ShowPanelSfx { get; private set; }

    [field: Header("State")]
    [field: SerializeField]
    public bool IsPanelVisible { get; private set; }

    [field: SerializeField]
    public DSDialogueSO CurrentDialogNode { get; private set; }

    Sequence _showHidePanelTween;
    Sequence _showTextTween;

    private void Start() {
      CreateTweens();
      ResetPanel();
    }

    private void CreateTweens() {
      _showHidePanelTween =
          DOTween.Sequence()
              .SetTarget(PanelRectTransform)
              .Insert(0f, PanelCanvasGroup.DOFade(1f, 0.3f))
              .Insert(0f, PanelRectTransform.DOLocalMove(new(0f, 25f, 0f), 0.3f).From(false, true))
              .Insert(0f, PortraitImage.transform.DOPunchScale(Vector3.one * 0.05f, 0.3f, 0, 0f))
              .Insert(0f, DialogText.transform.DOLocalMoveY(5f, 0.3f).From(true))
              .SetAutoKill(false)
              .Pause();

      _showTextTween =
          DOTween.Sequence()
              .SetTarget(PanelRectTransform)
              .Insert(0f, PortraitImage.transform.DOPunchScale(Vector3.one * 0.05f, 0.3f, 0, 0f))
              .Insert(0f, DialogText.transform.DOLocalMoveY(5f, 0.3f).From(true))
              .SetAutoKill(false)
              .Pause();
    }

    public void ResetPanel() {
      PanelCanvasGroup.alpha = 0f;
      PanelCanvasGroup.blocksRaycasts = false;

      IsPanelVisible = false;
    }

    public void TogglePanel() {
      if (IsPanelVisible) {
        HidePanel();
      } else {
        ShowPanel();
      }
    }

    public void ShowPanel() {
      PanelCanvasGroup.blocksRaycasts = true;
      IsPanelVisible = true;

      _showHidePanelTween.PlayAgain();
      SfxAudioSource.PlayOneShot(ShowPanelSfx);
    }

    public void HidePanel() {
      PanelCanvasGroup.blocksRaycasts = false;
      IsPanelVisible = false;

      _showHidePanelTween.SmoothRewind();
    }

    public void OnConfirmButtonClick() {
      StopSfxAudioClip();
      ShowDialogNode(GetNextDialogNode(CurrentDialogNode));
    }

    public void ShowDialogNode(DSDialogueSO dialogNode) {
      StopSfxAudioClip();

      CurrentDialogNode = dialogNode;

      if (!CurrentDialogNode) {
        HidePanel();
        return;
      }

      DialogText.text = CurrentDialogNode.Text;

      if (CurrentDialogNode.Portrait) {
        PortraitImage.sprite = CurrentDialogNode.Portrait;
      }

      if (IsPanelVisible) {
        _showTextTween.Play();
      } else {
        ShowPanel();
      }

      if (CurrentDialogNode.AudioClip) {
        PlaySfxAudioClip(CurrentDialogNode.AudioClip, CurrentDialogNode.AudioVolume);
      }
    }

    private DSDialogueSO GetNextDialogNode(DSDialogueSO dialogNode) {
      if (!dialogNode || dialogNode.Choices.Count <= 0) {
        return default;
      }

      return dialogNode.Choices[0].NextDialogue;
    }

    private void PlaySfxAudioClip(AudioClip audioClip, float audioVolume = 1f) {
      SfxAudioSource.clip = audioClip;
      SfxAudioSource.volume = audioVolume;
      SfxAudioSource.PlayDelayed(delay: 0.25f);
    }

    private void StopSfxAudioClip() {
      if (SfxAudioSource.isPlaying) {
        SfxAudioSource.Stop();
      }
    }
  }
}
