using DS.Events;

using UnityEngine;

using YoloBox;

namespace GameJam {
  public sealed class EventManager : SingletonManager<EventManager> {
    [field: Header("Agents")]
    [field: SerializeField]
    public GameObject PlayerTarget { get; private set; }

    [field: SerializeField]
    public SparrowController Sparrow { get; private set; }

    // TODO: make this not terrible because it will become a giant switch statement at some point.
    public void ProcessDialogEvent(DSDialogEvent dialogEvent) {
      Debug.Log($"Processing event: {dialogEvent.EventName}");

      switch (dialogEvent.EventType) {
        case DSEventType.SparrowFollowPlayer:
          Sparrow.StartFollowingTarget(PlayerTarget);
          break;
      }
    }
  }
}
