using System.Collections;

using DS.ScriptableObjects;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class GameManager : SingletonManager<GameManager> {
    [field: SerializeField]
    public DSDialogueSO StartingDialogNode { get; private set; }

    private void Start() {
      UIManager.Instance.CalloutPanels(calloutDuration: 5f);

      StartCoroutine(ShowStartingDialog(5f));
    }

    private IEnumerator ShowStartingDialog(float showDelay) {
      yield return new WaitForSeconds(seconds: showDelay);
      SetDialogNode(StartingDialogNode);
    }

    public void SetDialogNode(DSDialogueSO dialogNode) {
      UIManager.Instance.SetCurrentDialogNode(dialogNode);
    }

    public void SetUserCount(int userCount) {
      UIManager.Instance.SetCurrentUserCount(userCount);
    }
  }
}
