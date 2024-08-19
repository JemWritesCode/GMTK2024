using System.Collections;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class WorldAdManager : SingletonManager<WorldAdManager> {
    [field: Header("AdPrefab")]
    [field: SerializeField]
    public GameObject WorldAdPrefab { get; private set; }

    [field: Header("Ads")]
    [field: SerializeField]
    public Sprite[] AdSprites { get; private set; }

    public WorldAdController CreateWorldAd(Vector3 position, Quaternion rotation) {
      return CreateWorldAd(GetRandomAdSprite(), position, rotation);
    }

    public WorldAdController CreateWorldAd(Sprite adSprite, Vector3 position, Quaternion rotation) {
      GameObject worldAd = Instantiate(WorldAdPrefab, position, rotation);
      worldAd.name = $"WorldAd";

      worldAd.TryGetComponent(out WorldAdController worldAdController);
      worldAdController.AdSpriteRenderer.enabled = false;
      worldAdController.AdSpriteRenderer.sprite = GetRandomAdSprite();

      return worldAdController;
    }

    public Sprite GetRandomAdSprite() {
      return AdSprites[Random.Range(0, AdSprites.Length)];
    }

    public IEnumerator ShowAdsRandomly() {
      WaitForSeconds waitInterval = new(5f);

      while (true) {
        yield return waitInterval;

        Transform agentTransform = InteractManager.Instance.InteractAgent.transform;
        Vector3 targetPosition = agentTransform.position + agentTransform.forward * 2f;
        Vector3 targetRotation = new(0f, agentTransform.rotation.eulerAngles.y, 0f);

        WorldAdController worldAdController = CreateWorldAd(targetPosition, Quaternion.Euler(targetRotation));

        worldAdController.DisplayAd(Random.Range(5f, 10f));
      }
    }
  }
}
