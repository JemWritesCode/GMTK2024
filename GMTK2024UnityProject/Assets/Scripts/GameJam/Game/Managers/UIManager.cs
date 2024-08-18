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
      HelpPanel.TogglePanel();
    }

    public void SetCurrentInteractable(Interactable interactable) {
      InteractPanel.SetInteractable(interactable);
    }

    public void SetCurrentServer(Server server) {
      ServerPanel.SetServer(server);
    }
  }
}
