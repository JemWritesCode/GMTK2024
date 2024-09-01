using UnityEngine;

using static UnityEngine.GraphicsBuffer;

namespace GameJam {
  public sealed class SparrowController : MonoBehaviour {
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
  }
}
