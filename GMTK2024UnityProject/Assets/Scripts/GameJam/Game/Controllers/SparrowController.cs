using System.Collections.Generic;

using DS.ScriptableObjects;

using UnityEngine;

using static GameJam.SparrowAnimatorController;

namespace GameJam {
  public sealed class SparrowController : MonoBehaviour {
    [field: Header("Interact")]
    [field: SerializeField]
    public DSDialogueSO OnInteractDialogNode { get; set; }

    [field: SerializeField]
    public GameObject CarryingAttachPoint { get; set; }

    [field: SerializeField]
    public List<CableStartPoint> CarryingCables { get; set; } = new();

    [field: Header("Follow")]
    [field: SerializeField]
    public GameObject TargetToFollow { get; set; }

    [field: SerializeField]
    public float BufferDistance { get; set; }

    [field: SerializeField]
    public float RunDistance { get; set; }

    [field: SerializeField]
    public float FollowSpeed { get; set; }

    [field: SerializeField]
    public float FollowRunSpeed { get; set; }

    [field: SerializeField]
    public float RotateSpeed { get; set; }

    [field: SerializeField]
    public bool IsFlying { get; set; }

    [field: SerializeField]
    public float FlySpeed { get; set; }

    [field: Header("Animator")]
    [field: SerializeField]
    public SparrowAnimatorController SparrowAnimator { get; private set; }

    private void Update() {
      float deltaTime = Time.deltaTime;

      Vector3 direction = TargetToFollow.transform.position - transform.position;
      direction.y = 0f;

      float distance = direction.magnitude;
      MovementState movementState;

      if (distance > BufferDistance) {
        direction.Normalize();

        if (distance > RunDistance) {
          transform.position += FollowRunSpeed * deltaTime * direction;
          movementState = MovementState.Rolling;
        } else {
          transform.position += FollowSpeed * deltaTime * direction;
          movementState = MovementState.Walking;
        }
      } else {
        movementState = MovementState.Idle;
      }

      if (IsFlying) {
        movementState = MovementState.Flying;
      }

      SparrowAnimator.SetMovementState(movementState);

      Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * deltaTime);
    }

    public void OnInteract(GameObject interactAgent) {
      if (HandManager.Instance.HoldingCable()) {
        CableStartPoint cable = HandManager.Instance.CurrentCable;

        if (TryAddCarryingCable(cable)) {
          HandManager.Instance.CurrentCable = default;
          cable.SetPickedUpTarget(CarryingAttachPoint, 0f);
        }
      } else if (!HandManager.Instance.HoldingItem()) {
        if (TryGetCarryingCable(out CableStartPoint cable)) {
          HandManager.Instance.CurrentCable = cable;;
          HandManager.Instance.CurrentCable.SetPickedUpTarget(interactAgent);
        } else {
          GameManager.Instance.SetDialogNode(OnInteractDialogNode);
        }
      }
    }

    private bool TryGetCarryingCable(out CableStartPoint cable) {
      if (CarryingCables.Count > 0) {
        cable = CarryingCables[0];
        CarryingCables.RemoveAt(0);
        return true;
      }

      cable = default;
      return false;
    }

    private bool TryAddCarryingCable(CableStartPoint cable) {
      // We can set a limit here in the future, for now unlimited.
      CarryingCables.Add(cable);
      return true;
    }
  }
}
