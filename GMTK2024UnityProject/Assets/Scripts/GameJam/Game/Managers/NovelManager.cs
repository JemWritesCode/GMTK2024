using System.Collections;

using DS.ScriptableObjects;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class NovelManager : SingletonManager<NovelManager> {
    [field: Header("Controllers")]
    [field: SerializeField]
    public NovelSceneUIController UIController { get; private set; }

    [field: Header("State")]
    [field: SerializeField]
    public DSDialogueSO StartingDialogNode { get; private set; }

    private IEnumerator Start() {
      yield return new WaitForSeconds(seconds: 2f);
      UIController.DialogPanel.ShowDialogNode(StartingDialogNode);
    }
  }
}
