using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class UIManager : SingletonManager<UIManager> {
    [field: Header("Panels")]
    [field: SerializeField]
    public MenuPanelController MenuPanel { get; private set; }

    [field: SerializeField]
    public InteractPanelController InteractPanel { get; private set; }

    [field: SerializeField]
    public ServerPanelController ServerPanel { get; private set; }

    public bool ShouldUnlockCursor() {
      return MenuPanel.IsPanelVisible;
    }

    public void SetCurrentInteractable(Interactable interactable) {
      InteractPanel.SetInteractable(interactable); 
      ServerPanel.SetServer(interactable ? interactable.GetComponentInParent<Server>() : default);
    }
  }
}
