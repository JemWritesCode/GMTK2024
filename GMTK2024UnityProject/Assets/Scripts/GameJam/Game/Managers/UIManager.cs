using System.Collections;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class UIManager : SingletonManager<UIManager> {
    [field: Header("Panels")]
    [field: SerializeField]
    public MenuPanelController MenuPanel { get; private set; }

    [field: SerializeField]
    public HelpPanelController HelpPanel { get; private set; }

    [field: SerializeField]
    public InteractPanelController InteractPanel { get; private set; }

    [field: SerializeField]
    public ServerPanelController ServerPanel { get; private set; }

    private void Start() {
      SetupButtonListeners();
    }

    private void SetupButtonListeners() {
      MenuPanel.CloseButton.onClick.AddListener(MenuPanel.HidePanel);
      MenuPanel.CloseButton.onClick.AddListener(HelpPanel.HidePanel);
    }

    public bool ShouldUnlockCursor() {
      return MenuPanel.IsPanelVisible;
    }

    public void ToggleMenu() {
      MenuPanel.TogglePanel();

      if (MenuPanel.IsPanelVisible != HelpPanel.IsPanelVisible) {
        HelpPanel.TogglePanel();
      }
    }

    public void CalloutHelpPanel(float calloutDuration) {
      StartCoroutine(ShowHideHelpPanel(calloutDuration));
    }

    private IEnumerator ShowHideHelpPanel(float hideDelay) {
      yield return new WaitForSeconds(seconds: 0.5f);
      HelpPanel.ShowPanel();

      yield return new WaitForSeconds(seconds: hideDelay);

      if (!MenuPanel.IsPanelVisible) {
        HelpPanel.HidePanel();
      }
    }

    public void SetCurrentInteractable(Interactable interactable) {
      InteractPanel.SetInteractable(interactable);
    }

    public void SetCurrentServer(Server server) {
      ServerPanel.SetServer(server);
    }
  }
}
