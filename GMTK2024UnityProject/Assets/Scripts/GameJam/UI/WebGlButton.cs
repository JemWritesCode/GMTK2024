using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameJam {
  public sealed class WebGlButton : Button {
    [field: SerializeField]
    public UnityEvent OnPointerDownClick { get; set; }

    public override void OnPointerDown(PointerEventData eventData) {
      base.OnPointerDown(eventData);

      OnPointerDownClick.Invoke();
    }
  }
}
