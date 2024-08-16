using UnityEngine;

namespace YoloBox {
  public abstract class SingletonManager<T> : MonoBehaviour where T : Component {
    static T _instance;

    public static T Instance {
      get {
        if (!_instance) {
          _instance = FindObjectOfType<T>();
        }

        return _instance;
      }
    }

    protected virtual void Awake() {
      if (_instance) {
        Destroy(gameObject);
      } else {
        _instance = this as T;
      }
    }
  }
}
