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

    [field: SerializeField]
    public KeyCode InteractKey { get; private set; } = KeyCode.E;

    private void Update() {
      if (Input.GetKeyDown(ToggleMenuKey)) {
        OnToggleMenuKey();
      }

      if (Input.GetKeyDown(InteractKey)) {
        OnInteractKey();
      }

      UpdateCursorLockState();
    }

    public void OnToggleMenuKey() {
      UIManager.Instance.MenuPanel.TogglePanel();
    }

    public void OnInteractKey() {
      Interactable interactable = InteractManager.Instance.ClosestInteractable;

      if (interactable) {
        Debug.Log($"Has interactable, interacting.");
        interactable.Interact(InteractManager.Instance.InteractAgent);
      }
    }

    public bool IsCursorLocked { get; private set; }

    public void UpdateCursorLockState() {
      bool shouldUnlockCursor = ShouldUnlockCursor();

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

    public bool ShouldUnlockCursor() {
      return UIManager.Instance && UIManager.Instance.ShouldUnlockCursor();
    }

    public void LockCursor() {
      IsCursorLocked = true;

      if (PlayerCharacterController) {
        PlayerCharacterController.UnpausePlayer();
      }

      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }

    public void UnlockCursor() {
      IsCursorLocked = false;

      if (PlayerCharacterController) {
        PlayerCharacterController.PausePlayer(PauseModes.BlockInputOnly);
      }

      Cursor.lockState = CursorLockMode.Confined;
      Cursor.visible = true;
    }
  }
}
