using UnityEngine;

using static GameJam.CableType;

namespace GameJam {
  public class CableEndPoint : MonoBehaviour {
    public CableStartPoint Connection;
    public CableBoxType Type = CableBoxType.None;
    public AudioClip cableEndPointSound;
    public AudioClip wrongCableTypeSound;
    AudioSource cableEndPointAudioSource;

    public GameObject CableAttachPoint;
    public GameObject FlameEffects;
    public GameObject DataEffects;

    [field: Header("Color")]
    [field: SerializeField]
    public Renderer EndPointRenderer { get; private set; }

    [field: SerializeField]
    public Color UnconnectedColor { get; private set; } = Color.white;

    [field: SerializeField]
    public Color DataConnectedColor { get; private set; } = Color.cyan;

    [field: SerializeField]
    public Color PowerConnectedColor { get; private set; } = Color.yellow;

    private static readonly int _colorShaderId = Shader.PropertyToID("_Color");
    private static MaterialPropertyBlock _propertyBlock;
    private static MaterialPropertyBlock GetSharedPropertyBlock() {
      return _propertyBlock ??= new();
    }

    private void Start() {
      cableEndPointAudioSource = GetComponent<AudioSource>();
      SetEndPointColor(UnconnectedColor);
    }

    private void OnValidate() {
      SetEndPointColor(UnconnectedColor);
    }

    public void SetEndPointColor(Color color) {
      MaterialPropertyBlock propertyBlock = GetSharedPropertyBlock();
      EndPointRenderer.GetPropertyBlock(propertyBlock);
      propertyBlock.SetColor(_colorShaderId, color);
      EndPointRenderer.SetPropertyBlock(propertyBlock);
    }

    private void RefreshEndPointColor() {
      if (IsConnected()) {
        Color color =
            Type switch {
              CableBoxType.Data => DataConnectedColor,
              CableBoxType.Power => PowerConnectedColor,
              _ => Color.white
            };

        SetEndPointColor(color);
      } else {
        SetEndPointColor(UnconnectedColor);
      }
    }

    public Vector3 GetCableAttachPoint() {
      if (CableAttachPoint == null) {
        return this.transform.position;
      }

      return CableAttachPoint.transform.position;
    }

    public bool IsConnected() {
      return Connection != null;
    }

    public void BreakConnection(bool playEffects) {
      if (IsConnected()) {
        Connection.BreakConnection();

        if (playEffects) {
          PlayBreakConnectionEffects();
        }
      }

      RefreshEndPointColor();
    }

    private void PlayBreakConnectionEffects() {
      ParticleSystem[] effects = null;

      if (Type == CableBoxType.Data) {
        effects = DataEffects.GetComponentsInChildren<ParticleSystem>();
      } else if (Type == CableBoxType.Power) {
        effects = FlameEffects.GetComponentsInChildren<ParticleSystem>();
      }

      if (effects != null) {
        foreach (var effect in effects) {
          effect.Play();
        }
      }
    }

    public void CancelConnection() {
      if (IsConnected()) {
        Connection.StartConnection();
        Connection = null;
      }

      RefreshEndPointColor();
    }

    public void CompleteConnection(CableStartPoint cable) {
      Connection = cable;
      cable.CompleteConnection(this);
      if (cableEndPointSound && cableEndPointAudioSource) {
        cableEndPointAudioSource.PlayOneShot(cableEndPointSound);
      }

      RefreshEndPointColor();
    }

    public void CableInteract(GameObject interactAgent) {
      if (HandManager.Instance.HoldingCable()) {

        if (!IsConnected() && HandManager.Instance.CurrentCable.Type == Type) {
          CompleteConnection(HandManager.Instance.CurrentCable);
        } else if (!(HandManager.Instance.CurrentCable.Type == Type)) {
          cableEndPointAudioSource.PlayOneShot(wrongCableTypeSound, .5f);
        }
      } else if (IsConnected()) {
        CancelConnection();
      }
    }
  }
}
