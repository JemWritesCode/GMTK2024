using DG.Tweening;

using DS.Enumerations;
using DS.ScriptableObjects;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class DialogPanelController : MonoBehaviour {
    [field: Header("Panel")]
    [field: SerializeField]
    public RectTransform PanelRectTransform { get; private set; }

    [field: SerializeField]
    public CanvasGroup PanelCanvasGroup { get; private set; }

    [field: Header("Display")]
    [field: SerializeField]
    public DialogDisplayController LeftPortraitDisplay { get; private set; }

    [field: SerializeField]
    public DialogDisplayController TopPortraitDisplay { get; private set; }

    [field: SerializeField]
    public DialogDisplayController AdHorizontalDisplay { get; private set; }

    [field: SerializeField]
    public DialogDisplayController AdVerticalDisplay { get; private set; }

    [field: SerializeField]
    public DialogDisplayController BigPortraitDisplay { get; private set; }

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

    [field: SerializeField]
    public DialogDisplayController CurrentDialogDisplay { get; private set; }

    Sequence _showHidePanelTween;

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
              .SetAutoKill(false)
              .SetUpdate(isIndependentUpdate: true)
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

      SetupDialogDisplay(dialogNode);

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

    private void SetupDialogDisplay(DSDialogueSO dialogNode) {
      DOTween.Complete(PanelRectTransform, withCallbacks: true);

      Sequence sequence =
          DOTween.Sequence()
              .SetTarget(PanelRectTransform)
              .SetUpdate(isIndependentUpdate: true);

      if (!IsPanelVisible) {
        sequence.AppendCallback(ShowPanel);
      }

      if (dialogNode.PortraitType != CurrentDialogDisplay.DisplayPortraitType) {
        DialogDisplayController dialogDisplay = CurrentDialogDisplay;

        sequence
            .AppendCallback(dialogDisplay.HideDisplay)
            .AppendInterval(0.15f);
      }

      if (dialogNode.PortraitType == DSPortraitType.LeftPortrait) {
        CurrentDialogDisplay = LeftPortraitDisplay;
        LeftPortraitDisplay.SetupDisplay(dialogNode);
        sequence.AppendCallback(LeftPortraitDisplay.ShowDisplay);
      } else if (dialogNode.PortraitType == DSPortraitType.TopPortrait) {
        CurrentDialogDisplay = TopPortraitDisplay;
        TopPortraitDisplay.SetupDisplay(dialogNode);
        sequence.AppendCallback(TopPortraitDisplay.ShowDisplay);
      } else if (dialogNode.PortraitType == DSPortraitType.AdHorizontal) {
        CurrentDialogDisplay = AdHorizontalDisplay;
        AdHorizontalDisplay.SetupDisplay(dialogNode);
        sequence.AppendCallback(AdHorizontalDisplay.ShowDisplay);
      } else if (dialogNode.PortraitType == DSPortraitType.AdVertical) {
        CurrentDialogDisplay = AdVerticalDisplay;
        AdVerticalDisplay.SetupDisplay(dialogNode);
        sequence.AppendCallback(AdVerticalDisplay.ShowDisplay);
      } else if (dialogNode.PortraitType == DSPortraitType.BigPortrait) {
        CurrentDialogDisplay = BigPortraitDisplay;
        BigPortraitDisplay.SetupDisplay(dialogNode);
        sequence.AppendCallback(BigPortraitDisplay.ShowDisplay);
      }
    }

    private void PlaySfxAudioClip(AudioClip audioClip, float audioVolume = 1f) {
      AudioSource audioSource = AudioManager.Instance.DialogAudioSource;
      audioSource.clip = audioClip;
      audioSource.volume = Mathf.Min(audioVolume, AudioManager.Instance.DialogVolume);
      audioSource.PlayDelayed(delay: 0.25f);
    }

    private void StopSfxAudioClip() {
      AudioSource audioSource = AudioManager.Instance.DialogAudioSource;

      if (audioSource.isPlaying) {
        audioSource.Stop();
      }
    }
  }
}
