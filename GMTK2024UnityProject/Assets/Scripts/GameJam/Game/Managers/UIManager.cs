using System.Collections;

using DG.Tweening;

using DS;
using DS.ScriptableObjects;

using UnityEngine;
using UnityEngine.UI;

using YoloBox;

namespace GameJam {
  public sealed class UIManager : SingletonManager<UIManager> {
    [field: Header("Panels")]
    [field: SerializeField]
    public MenuPanelController MenuPanel { get; private set; }

    [field: SerializeField]
    public HelpPanelController HelpPanel { get; private set; }

    [field: SerializeField]
    public UserCountPanelController UserCountPanel { get; private set; }

    [field: SerializeField]
    public DialogPanelController DialogPanel { get; private set; }

    [field: SerializeField]
    public InteractPanelController InteractPanel { get; private set; }

    [field: SerializeField]
    public ServerPanelController ServerPanel { get; private set; }

    [field: Header("Overlays")]
    [field: SerializeField]
    public OverlayController DarkOverlay { get; private set; }

    private void Start() {
      SetupButtonListeners();
    }

    private void SetupButtonListeners() {
      MenuPanel.CloseButton.onClick.AddListener(MenuPanel.HidePanel);
      MenuPanel.CloseButton.onClick.AddListener(HelpPanel.HidePanel);
    }

    public bool ShouldUnlockCursor() {
      return MenuPanel.IsPanelVisible || DialogPanel.IsPanelVisible;
    }

    public void ToggleMenu() {
      MenuPanel.TogglePanel();

      if (MenuPanel.IsPanelVisible != HelpPanel.IsPanelVisible) {
        HelpPanel.TogglePanel();
      }
    }

    public void ToggleDarkOverlay(bool toggleOn) {
      if (DarkOverlay.IsOverlayVisible != toggleOn) {
        DarkOverlay.ToggleOverlay(toggleOn);
      }
    }

    public void CalloutPanels(float calloutDuration) {
      StartCoroutine(ShowHideHelpPanel(0.5f, calloutDuration));
      StartCoroutine(ShowUserCountPanel(1.5f));
    }

    private IEnumerator ShowHideHelpPanel(float showDelay, float hideDelay) {
      yield return new WaitForSeconds(seconds: showDelay);
      HelpPanel.ShowPanel();

      yield return new WaitForSeconds(seconds: hideDelay);

      if (!MenuPanel.IsPanelVisible) {
        HelpPanel.HidePanel();
      }
    }

    private IEnumerator ShowUserCountPanel(float showDelay) {
      yield return new WaitForSeconds(seconds: showDelay);
      UserCountPanel.ShowPanel();
    }

    public void SetCurrentInteractable(Interactable interactable) {
      InteractPanel.SetInteractable(interactable);
    }

    public void SetCurrentServer(Server server) {
      ServerPanel.SetServer(server);
    }

    public void SetCurrentUserCount(int userCount) {
      UserCountPanel.SetUserCountValue(userCount);
    }

    public void SetCurrentDialogNode(DSDialogueSO dialogNode) {
      DialogPanel.ShowDialogNode(dialogNode);
    }
  }
}
