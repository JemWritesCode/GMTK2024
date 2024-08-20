using System.Collections;

using DS.ScriptableObjects;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class GameManager : SingletonManager<GameManager> {
    [field: SerializeField]
    public DSDialogueSO StartingDialogNode { get; private set; }

    [field: Header("Pause")]
    [field: SerializeField]
    public bool IsGamePaused { get; private set; }

    private void Start() {
      StartCoroutine(StartGame());
    }

    private IEnumerator StartGame() {
      yield return null;

      UIManager.Instance.CalloutPanels(calloutDuration: 8f);

      if (StartingDialogNode) {
        SetDialogNode(StartingDialogNode);
      }
    }

    public void SetDialogNode(DSDialogueSO dialogNode) {
      UIManager.Instance.SetCurrentDialogNode(dialogNode);
    }

    public void SetUserCount(int userCount) {
      UIManager.Instance.SetCurrentUserCount(userCount);
    }

    private void Update() {
      SetIsGamePaused(ShouldPauseGame());
    }

    public void SetIsGamePaused(bool toggleOn) {
      if (toggleOn && !IsGamePaused) {
        IsGamePaused = true;
        Time.timeScale = 0f;
      } else if (!toggleOn && IsGamePaused) {
        IsGamePaused = false;
        Time.timeScale = 1f;
      }
    }

    public bool ShouldPauseGame() {
      return UIManager.Instance.MenuPanel.IsPanelVisible || UIManager.Instance.DialogPanel.IsPanelVisible;
    }
  }
}
