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
    public CableStartPoint CarryingCable { get; set; }

    [field: Header("Follow")]
    [field: SerializeField]
    public GameObject TargetToFollow { get; set; }

    [field: SerializeField]
    public float BufferDistance { get; set; }

    [field: SerializeField]
    public float FollowSpeed { get; set; }

    [field: SerializeField]
    public float RotateSpeed { get; set; }

    private void Update() {
      Vector3 direction = TargetToFollow.transform.position - transform.position;
      direction.y = 0f;

      float distance = direction.magnitude;

      if (distance > BufferDistance) {
        direction.Normalize();
        transform.position += FollowSpeed * Time.deltaTime * direction;
      }

      Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
    }

    public void OnInteract(GameObject interactAgent) {
      if (HandManager.Instance.HoldingCable()) {
        if (CarryingCable) {
          // ...
        } else {
          CarryingCable = HandManager.Instance.CurrentCable;
          HandManager.Instance.CurrentCable = default;
          CarryingCable.SetPickedUpTarget(CarryingAttachPoint, 0f);
        }
      } else if (!HandManager.Instance.HoldingItem()) {
        if (CarryingCable) {
          HandManager.Instance.CurrentCable = CarryingCable;
          CarryingCable = default;
          HandManager.Instance.CurrentCable.SetPickedUpTarget(interactAgent);
        } else {
          GameManager.Instance.SetDialogNode(OnInteractDialogNode);
        }
      }
    }
  }
}
