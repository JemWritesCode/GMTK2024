using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class InputManager : SingletonManager<InputManager> {
    [field: SerializeField]
    public KeyCode ToggleMenuKey { get; set; } = KeyCode.Tab;

    private void Update() {
      if (Input.GetKeyDown(ToggleMenuKey)) {
        OnToggleMenuKey();
      }
    }

    public void OnToggleMenuKey() {

    }
  }
}
