using YoloBox;

namespace GameJam {
  public sealed class GameManager : SingletonManager<GameManager> {
    private void Start() {
      UIManager.Instance.CalloutHelpPanel(calloutDuration: 5f);
    }
  }
}
