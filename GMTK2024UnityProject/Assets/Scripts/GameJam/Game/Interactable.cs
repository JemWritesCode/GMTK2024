using UnityEngine;
using UnityEngine.Events;

namespace GameJam {
  public class Interactable : MonoBehaviour {
    [field: SerializeField, Header("Interact")]
    public float InteractRange { get; set; } = 2f;

    [field: SerializeField]
    public bool CanInteract { get; set; } = true;

    [field: SerializeField]
    public string InteractText { get; set; } = string.Empty;

    [field: SerializeField, Header("Highlight")]
    public Renderer HighlightRenderer { get; set; }

    [Header("Events")]
    [Space(10f)]
    public UnityEvent<GameObject> OnInteract;

    public void Interact() {
      OnInteract?.Invoke(default);
    }

    public void Interact(GameObject interactAgent) {
      OnInteract?.Invoke(interactAgent);
    }
  }
}
