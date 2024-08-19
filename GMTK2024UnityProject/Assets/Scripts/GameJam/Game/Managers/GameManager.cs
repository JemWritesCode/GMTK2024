using System.Collections;

using DS.ScriptableObjects;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class GameManager : SingletonManager<GameManager> {
    [field: SerializeField]
    public DSDialogueSO StartingDialogNode { get; private set; }

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
  }
}
