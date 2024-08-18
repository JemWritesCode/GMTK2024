using System;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class InteractManager : SingletonManager<InteractManager> {
    [field: Header("Agent")]
    [field: SerializeField]
    public GameObject InteractAgent { get; private set; }

    [field: SerializeField, Min(0f)]
    public float InteractRange { get; set; } = 4f;

    [field: Header("Interact")]
    [field: SerializeField]
    public bool CanInteract { get; set; } = true;

    [field: SerializeField]
    public Interactable ClosestInteractable { get; private set; }

    private void FixedUpdate() {
      Interactable interactable =
          CanInteract && InteractAgent ? GetClosestInteractable(InteractAgent.transform, InteractRange) : default;

      if (interactable != ClosestInteractable) {
        HighlightManager.Instance.RemoveInteractable(ClosestInteractable);
        HighlightManager.Instance.AddInteractable(interactable);

        ClosestInteractable = interactable;
      }

      UIManager.Instance.SetCurrentInteractable(ClosestInteractable);
    }

    private readonly RaycastHit[] _raycastHits = new RaycastHit[20];
    private readonly float[] _hitDistanceCache = new float[20];

    private Interactable GetClosestInteractable(Transform origin, float range) {
      int count =
          Physics.SphereCastNonAlloc(
              origin.position,
              0.25f,
              origin.forward,
              _raycastHits,
              range,
              -1,
              QueryTriggerInteraction.Ignore);

      if (count <= 0) {
        return default;
      }

      for (int i = 0; i < count; i++) {
        _hitDistanceCache[i] = _raycastHits[i].distance;
      }

      Array.Sort(_hitDistanceCache, _raycastHits, 0, count);

      for (int i = 0; i < count; i++) {
        RaycastHit raycastHit = _raycastHits[i];
        Interactable interactable = raycastHit.collider.GetComponentInParent<Interactable>();

        if (interactable
            && interactable.enabled
            && interactable.CanInteract
            && raycastHit.distance <= interactable.InteractRange) {
          return interactable;
        }
      }

      return default;
    }
  }
}
