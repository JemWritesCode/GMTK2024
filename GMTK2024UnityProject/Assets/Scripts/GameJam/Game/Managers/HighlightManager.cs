using Facepunch;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class HighlightManager : SingletonManager<HighlightManager> {
    [field: SerializeField]
    public Highlight InteractHighlight { get; private set; }

    public void AddInteractable(Interactable interactable) {
      if (interactable && interactable.HighlightRenderer) {
        InteractHighlight.Add(interactable.HighlightRenderer);
        InteractHighlight.Rebuild();
      }
    }

    public void RemoveInteractable(Interactable interactable) {
      if (interactable && interactable.HighlightRenderer) {
        InteractHighlight.Remove(interactable.HighlightRenderer);
        InteractHighlight.Rebuild();
      }
    }
  }
}
