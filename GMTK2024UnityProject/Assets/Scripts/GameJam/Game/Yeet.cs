using System.Collections;

using UnityEngine;

namespace GameJam {
  [RequireComponent(typeof(Rigidbody))]
  public sealed class Yeet : MonoBehaviour {
    [field: Header("YeetForce")]
    [field: SerializeField]
    public Rigidbody YeetRigidbody { get; set; }

    [field: SerializeField]
    public Vector3 YeetPower { get; set; }

    private void Start() {
      StartCoroutine(DelayedYeet(Random.Range(5f, 12f)));
    }

    public IEnumerator DelayedYeet(float delaySeconds) {
      yield return new WaitForSeconds(seconds: delaySeconds);
      YeetTheThing();
    }

    public void YeetTheThing() {
      Vector3 yeetForce = new(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
      yeetForce.Normalize();

      YeetRigidbody.AddTorque(yeetForce * 10f);

      yeetForce =
          new Vector3(
              Random.Range(2f, 4.5f) * (Random.Range(0, 2) * 2 - 1),
              Random.Range(2f, 4.5f),
              Random.Range(2f, 4.5f) * (Random.Range(0, 2) * 2 - 1));


      YeetRigidbody.AddForce(yeetForce + YeetPower, ForceMode.VelocityChange);
    }
  }
}
