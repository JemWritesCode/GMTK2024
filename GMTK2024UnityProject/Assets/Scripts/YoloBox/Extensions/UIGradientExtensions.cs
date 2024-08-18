using Coffee.UIEffects;

using UnityEngine;

namespace YoloBox {
  public static class UIGradientExtensions {
    public static Color GetColor1(this UIGradient gradient) {
      return gradient.color1;
    }

    public static Color GetColor2(this UIGradient gradient) {
      return gradient.color2;
    }

    public static void SetColor1(this UIGradient gradient, Color color1) {
      gradient.color1 = color1;
    }

    public static void SetColor2(this UIGradient gradient, Color color2) {
      gradient.color2 = color2;
    }
  }
}
