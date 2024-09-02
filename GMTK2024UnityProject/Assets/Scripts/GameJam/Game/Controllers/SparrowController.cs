using System.Collections.Generic;

using DS.ScriptableObjects;

using UnityEngine;

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

    [field: Header("Animator")]
    [field: SerializeField]
    public Animator SparrowAnimator { get; private set; }

    public int IsRolling = Animator.StringToHash("IsRolling");
    public int IsWalking = Animator.StringToHash("IsWalking");
    public int IsMoving = Animator.StringToHash("IsMoving");

    private void Update() {
      float deltaTime = Time.deltaTime;

      Vector3 direction = TargetToFollow.transform.position - transform.position;
      direction.y = 0f;

      float distance = direction.magnitude;

      if (distance > BufferDistance) {
        direction.Normalize();
        SparrowAnimator.SetBool(IsMoving, true);

        if (distance > RunDistance) {
          transform.position += FollowRunSpeed * deltaTime * direction;
          SparrowAnimator.SetBool(IsRolling, true);
          SparrowAnimator.SetBool(IsWalking, false);
        } else {
          transform.position += FollowSpeed * deltaTime * direction;
          SparrowAnimator.SetBool(IsRolling, false);
          SparrowAnimator.SetBool(IsWalking, true);
        }
      } else {
        SparrowAnimator.SetBool(IsMoving, false);
      }

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
