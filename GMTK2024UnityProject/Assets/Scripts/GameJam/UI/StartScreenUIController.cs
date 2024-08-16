using UnityEngine;
using UnityEngine.UI;

namespace GameJam {
  public sealed class StartScreenUIController : MonoBehaviour {
    [field: SerializeField]
    public Image GameLogo { get; private set; }
  }
}
