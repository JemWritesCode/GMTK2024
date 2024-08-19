using DG.Tweening;

using UnityEngine;

namespace GameJam {
  public sealed class WorldAdController : MonoBehaviour {
    [field: Header("Ad")]
    [field: SerializeField]
    public Transform AdTransform { get; private set; }

    [field: SerializeField]
    public SpriteRenderer AdSpriteRenderer { get; private set; }

    public void DisplayAd(float timeToLive) {
      AdSpriteRenderer.enabled = true;
      AdSpriteRenderer.DOFade(1f, 3f).From(0f, true);

      Invoke(nameof(DestroyAd), timeToLive);
    }

    public void DestroyAd() {
      Destroy(gameObject);
    }
  }
}
