using UnityEngine;
using UnityEngine.UI;

namespace GameJam {
  public sealed class NovelSceneUIController : MonoBehaviour {
    [field: Header("Background")]
    [field: SerializeField]
    public Image BackgroundImage { get; private set; }

    [field: Header("Buttons")]
    [field: SerializeField]
    public IconLabelButton StartButton { get; private set; }

    [field: Header("Dialog")]
    [field: SerializeField]
    public DialogPanelController DialogPanel { get; private set; }
  }
}
