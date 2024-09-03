using UnityEngine;

namespace GameJam {
  [RequireComponent(typeof(Animator))]
  public sealed class SparrowAnimatorController : MonoBehaviour {
    [field: Header("Animator")]
    [field: SerializeField]
    public Animator SparrowAnimator { get; private set; }

    public enum MovementState {
      Idle,
      Walking,
      Rolling,
      Flying
    }

    [field: Header("State")]
    [field: SerializeField]
    public MovementState CurrentMovementState { get; private set; } = MovementState.Idle;

    public static readonly int MovementStateHash = Animator.StringToHash("MovementState");

    private void OnEnable() {
      CurrentMovementState = (MovementState) SparrowAnimator.GetInteger(MovementStateHash);
    }

    public void SetMovementState(MovementState movementState) {
      if (CurrentMovementState != movementState) {
        CurrentMovementState = movementState;
        SparrowAnimator.SetInteger(MovementStateHash, (int) movementState);
      }
    }
  }
}
