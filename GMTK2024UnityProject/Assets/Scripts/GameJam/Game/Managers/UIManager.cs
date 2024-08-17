using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class UIManager : SingletonManager<UIManager> {
    [field: Header("Panels")]
    [field: SerializeField]
    public MenuPanelController MenuPanel { get; private set; }

    public bool ShouldUnlockCursor() {
      return MenuPanel.IsPanelVisible;
    } 
  }
}
