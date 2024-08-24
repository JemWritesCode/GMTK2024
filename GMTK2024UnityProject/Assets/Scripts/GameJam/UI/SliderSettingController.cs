using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace GameJam {
  public sealed class SliderSettingController : MonoBehaviour {
    [field: Header("UI")]
    [field: SerializeField]
    public TextMeshProUGUI SettingLabel { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI HandleValueLabel { get; private set; }

    [field: Header("Slider")]
    [field: SerializeField]
    public Slider SettingSlider { get; private set; }

    public enum SliderValueFormat {
      Float,
      WholeNumber,
      Percent,
    }

    [field: SerializeField]
    public SliderValueFormat HandleValueFormat { get; private set; }

    private void Start() {
      SetHandleValueLabel(SettingSlider.value);
    }

    public void SetValueWithoutNotify(float value) {
      SettingSlider.SetValueWithoutNotify(value);
      SetHandleValueLabel(value);
    }

    public void SetHandleValueLabel(float value) {
      HandleValueLabel.text =
          HandleValueFormat switch {
            SliderValueFormat.Float => $"{value:F2}",
            SliderValueFormat.WholeNumber => $"{Mathf.RoundToInt(value)}",
            SliderValueFormat.Percent => $"{Mathf.RoundToInt(value * 100f)}",
            _ => $"{value}",
          };
    }
  }
}
