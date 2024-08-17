using SUPERCharacter;

using System.Collections;

using UnityEngine;

namespace GameJam {
  public sealed class WebGlAdjustment : MonoBehaviour {
    [field: Header("SuperCharacterController")]
    [field: SerializeField]
    public SUPERCharacterAIO SuperCharacterController { get; private set; }

    [field: Header("Adjustments")]
    [field: SerializeField]
    public float MouseSensitivity { get; set; } = 2f;

    [field: SerializeField]
    public float BodyCatchupSpeed { get; set; } = 3f;

    void Start() {
      StartCoroutine(AdjustSensitivity());
    }

    public IEnumerator AdjustSensitivity() {
      yield return null;

#if UNITY_WEBGL && !UNITY_EDITOR
      Debug.Log("Adjusting sensitivity for WebGL.");
      SuperCharacterController.Sensitivity = MouseSensitivity;
      SuperCharacterController.bodyCatchupSpeed = BodyCatchupSpeed;
#endif
    }
  }
}
