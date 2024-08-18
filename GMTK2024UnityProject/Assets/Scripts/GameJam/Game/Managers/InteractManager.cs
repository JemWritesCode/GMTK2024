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

    [field: Header("Server")]
    [field: SerializeField]
    public float NearbyServerRadius { get; set; } = 2f;

    [field: SerializeField]
    public Server ClosestServer { get; private set; }

    private void FixedUpdate() {
      UpdateRaycastHits(InteractAgent, InteractRange);
      UpdateClosestInteractable();
      UpdateClosestServer();
    }

    private int _raycastHitsCount = 0;
    private readonly RaycastHit[] _raycastHits = new RaycastHit[20];
    private readonly float[] _hitDistanceCache = new float[20];

    private void UpdateRaycastHits(GameObject agent, float range) {
      if (!agent) {
        _raycastHitsCount = 0;
        return;
      }

      Transform origin = agent.transform;

      _raycastHitsCount =
          Physics.SphereCastNonAlloc(
              origin.position,
              0.25f,
              origin.forward,
              _raycastHits,
              range,
              -1,
              QueryTriggerInteraction.Ignore);

      for (int i = 0; i < _raycastHitsCount; i++) {
        _hitDistanceCache[i] = _raycastHits[i].distance;
      }

      Array.Sort(_hitDistanceCache, _raycastHits, 0, _raycastHitsCount);
    }

    private void UpdateClosestInteractable() {
      Interactable interactable = CanInteract ? GetClosestInteractable() : default;

      if (interactable != ClosestInteractable) {
        HighlightManager.Instance.RemoveInteractable(ClosestInteractable);
        HighlightManager.Instance.AddInteractable(interactable);

        ClosestInteractable = interactable;
      }

      UIManager.Instance.SetCurrentInteractable(ClosestInteractable);
    }

    private Interactable GetClosestInteractable() {
      for (int i = 0; i < _raycastHitsCount; i++) {
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

    private void UpdateClosestServer() {
      Server server = GetClosestServer();

      if (server != ClosestServer) {
        ClosestServer = server;
      }

      UIManager.Instance.SetCurrentServer(server);
    }

    private Server GetClosestServer() {
      for (int i = 0; i < _raycastHitsCount; i++) {
        RaycastHit raycastHit = _raycastHits[i];
        Server server = raycastHit.collider.GetComponentInParent<Server>();

        if (server && server.enabled && raycastHit.distance <= NearbyServerRadius) {
          return server;
        }
      }

      return default;
    }
  }
}
