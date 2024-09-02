using UnityEngine;

using static GameJam.CableType;

namespace GameJam {
  public class CableStartPoint : MonoBehaviour {
    public CableEndPoint Connection;
    public LineRenderer line;
    public CableType.CableBoxType Type = CableType.CableBoxType.None;
    public Material CableMaterial;
    public GameObject Hamster;
    public bool HasHamster = false;
    public GameObject CableAttachPoint;
    public float CableWidth = 0.025f;


    // SFX
    AudioSource cableStartPointAudioSource;
    public AudioClip grabCableSound;
    public AudioClip hamsterNibbleSound;

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

    private bool _pickedUp = false;

    private Transform _pickedUpTransform = default;
    private float _pickedUpForwardMultiplier = 1f;

    public void SetPickedUpTarget(GameObject target, float forwardMultiplier = 1f) {
      _pickedUpTransform = target.transform;
      _pickedUpForwardMultiplier = forwardMultiplier;
    }

    void Start() {
      if (line == null) {
        line = gameObject.AddComponent<LineRenderer>();
      }

      line.SetPosition(0, GetCableAttachPoint());
      line.SetPosition(1, GetCableAttachPoint());
      line.material = CableMaterial;
      line.startWidth = CableWidth;
      line.endWidth = CableWidth;
      line.useWorldSpace = true;

      cableStartPointAudioSource = GetComponent<AudioSource>();

      SetEndPointColor(UnconnectedColor);
    }

    void Update() {
      if (_pickedUp) {
        line.SetPosition(1, (_pickedUpTransform.forward * _pickedUpForwardMultiplier) + _pickedUpTransform.position);
      } else {
        RedrawCable();
      }
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

    public void BreakConnection() {
      if (Connection != null) {
        Connection.Connection = null;
        Connection = null;
      }

      _pickedUp = false;
      line.SetPosition(1, GetCableAttachPoint());

      RefreshEndPointColor();
    }

    public void CancelConnection() {
      BreakConnection();
      HandManager.Instance.CurrentCable = null;

      RefreshEndPointColor();
    }

    public void StartConnection(GameObject interactAgent) {
      HandManager.Instance.CurrentCable = this;
      BreakConnection();
      _pickedUp = true;
      SetPickedUpTarget(interactAgent);
      if (grabCableSound && cableStartPointAudioSource) {
        cableStartPointAudioSource.PlayOneShot(grabCableSound);
      }

      RefreshEndPointColor();
    }

    public void CompleteConnection(CableEndPoint cable) {
      _pickedUp = false;
      HandManager.Instance.CurrentCable = null;
      Connection = cable;
      line.SetPosition(1, Connection.GetCableAttachPoint());

      RefreshEndPointColor();
    }

    public void RedrawCable() {
      if (IsConnected()) {
        line.SetPosition(1, Connection.GetCableAttachPoint());
      }
    }

    public bool IsConnected() {
      return Connection != null;
    }

    public void CableInteract(GameObject interactAgent) {
      if (HandManager.Instance.TryPickup(this)) {
        if (!IsConnected()) {
          StartConnection(interactAgent);
        }
      } else if (HandManager.Instance.CurrentCable == this) {
        CancelConnection();
        return;
      }
    }

    public void HamsterAttack() {
      if (Hamster != null) {
        var obj = GameObject.Instantiate(Hamster, this.transform.position,
            this.transform.rotation * Hamster.transform.rotation);
        obj.transform.position = transform.position;
        obj.GetComponent<Hamster>().Lunch = this;
        HasHamster = true;
        if (hamsterNibbleSound && cableStartPointAudioSource) {
          cableStartPointAudioSource.PlayOneShot(hamsterNibbleSound, .4f);
        }
      }
    }
  }
}
