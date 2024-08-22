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
      UIManager.Instance.ToggleMenu();
    }

    public void OnInteractKey() {
      if (UIManager.Instance.ProcessInteractKey()) {
        return;
      }

      if (HandManager.Instance.ProcessInteractable()) {
        return;
      }

      Interactable interactable = InteractManager.Instance.ClosestInteractable;

      if (interactable) {
        interactable.Interact(InteractManager.Instance.InteractAgent);
      }
    }

    public bool IsCursorLocked { get; private set; }

    public void UpdateCursorLockState() {
      bool shouldUnlockCursor = ShouldUnlockCursor();

      if (shouldUnlockCursor) {
        UnlockCursor();
      } else {
        LockCursor();
      }
    }

    public bool ShouldUnlockCursor() {
      return UIManager.Instance && UIManager.Instance.ShouldUnlockCursor();
    }

    public void LockCursor() {
      if (!IsCursorLocked && PlayerCharacterController) {
        PlayerCharacterController.UnpausePlayer();
      }

      IsCursorLocked = true;

      if (Cursor.lockState != CursorLockMode.Locked) {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
      }
    }

    public void UnlockCursor() {
      if (IsCursorLocked && PlayerCharacterController) {
        PlayerCharacterController.PausePlayer(PauseModes.BlockInputOnly);
      }

      IsCursorLocked = false;

      if (Cursor.lockState != CursorLockMode.Confined) {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
      }
    }
  }
}
