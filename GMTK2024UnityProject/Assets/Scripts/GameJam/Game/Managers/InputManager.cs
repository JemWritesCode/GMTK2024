using SUPERCharacter;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class InputManager : SingletonManager<InputManager> {
    [field: Header("SuperCharacterController")]
    [field: SerializeField]
    public SUPERCharacterAIO PlayerCharacterController { get; private set; }

    [field: Header("Keybinds")]
    [field: SerializeField]
    public KeyCode ToggleMenuKey { get; set; } = KeyCode.Tab;

    private void Update() {
      if (Input.GetKeyDown(ToggleMenuKey)) {
        OnToggleMenuKey();
      }

      UpdateCursorLockState();
    }

    public void OnToggleMenuKey() {
      UIManager.Instance.MenuPanel.TogglePanel();
    }

    public bool IsCursorLocked { get; private set; }

    public void UpdateCursorLockState() {
      bool shouldUnlockCursor = UIManager.Instance.ShouldUnlockCursor();

      if (shouldUnlockCursor) {
        if (IsCursorLocked) {
          UnlockCursor();
        }
      } else {
        if (!IsCursorLocked) {
          LockCursor();
        }
      }
    }

    public void LockCursor() {
      Debug.Log("Locking cursor.");
      IsCursorLocked = true;

      if (PlayerCharacterController) {
        PlayerCharacterController.UnpausePlayer();
      }

      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }

    public void UnlockCursor() {
      Debug.Log("Unlocking cursor.");
      IsCursorLocked = false;

      if (PlayerCharacterController) {
        PlayerCharacterController.PausePlayer(PauseModes.BlockInputOnly);
      }

      Cursor.lockState = CursorLockMode.Confined;
      Cursor.visible = true;
    }
  }
}
