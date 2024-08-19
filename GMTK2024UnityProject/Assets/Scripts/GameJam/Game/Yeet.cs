using System;
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

    private Action Callback;

    public void StartYeet(Action callback) {
      Callback = callback;
      StartCoroutine(DelayedYeet(0)); //UnityEngine.Random.Range(0f, 0.5f)
    }

    public IEnumerator DelayedYeet(float delaySeconds) {
      yield return new WaitForSeconds(seconds: delaySeconds);
      YeetTheThing();
    }

    public void YeetTheThing() {
      Vector3 yeetForce = new(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
      yeetForce.Normalize();

      YeetRigidbody.AddTorque(yeetForce * 10f);

      yeetForce =
          new Vector3(
              UnityEngine.Random.Range(1f, 3f) * (UnityEngine.Random.Range(0, 2) * 2 - 1),
              UnityEngine.Random.Range(1f, 3f),
              UnityEngine.Random.Range(1f, 3f) * (UnityEngine.Random.Range(0, 2) * 2 - 1));


      YeetRigidbody.AddForce(yeetForce + YeetPower, ForceMode.VelocityChange);

      Callback?.Invoke();
      Callback = null;
    }
  }
}
