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

    private void Start() {
      UIController.StartButton.ButtonRef.OnPointerDownClick.AddListener(ShowStartingDialogNode);
    }

    private void ShowStartingDialogNode() {
      UIController.DialogPanel.ShowDialogNode(StartingDialogNode);
    }
  }
}
