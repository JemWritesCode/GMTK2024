using DS.ScriptableObjects;

using UnityEngine;
using UnityEngine.UI;

namespace GameJam {
  public sealed class NovelSceneUIController : MonoBehaviour {
    [field: Header("Background")]
    [field: SerializeField]
    public Image BackgroundImage { get; private set; }

    [field: Header("Dialog")]
    [field: SerializeField]
    public DialogPanelController DialogPanel { get; private set; }
  }
}
