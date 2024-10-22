using Coffee.UIEffects;

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

using TMPro;

using UnityEngine;

namespace YoloBox {
  public static class DOTWeenExtensions {
    public static void PlayComplete(this Tween tween) {
      tween.Complete();
      tween.Restart();
    }

    public static void PlayAgain(this Tween tween) {
      if (tween.IsPlaying()) {
        tween.PlayForward();
      } else {
        tween.Restart();
      }
    }

    public static void PlayOrRewind(this Tween tween, bool condition) {
      if (condition) {
        tween.PlayAgain();
      } else {
        tween.SmoothRewind();
      }
    }

    public static Tweener DOCounter(this TMP_Text target, int fromValue, int toValue, float duration) {
      return DOVirtual
          .Int(fromValue, toValue, duration, x => target.SetText(x.ToString()))
          .SetTarget(target);
    }

    public static Tweener DOPercentCounter(this TMP_Text target, float fromValue, float toValue, float duration) {
      return DOVirtual
          .Float(fromValue, toValue, duration, x => target.SetText($"{(x * 100f):F0}%"))
          .SetTarget(target);
    }

    public static TweenerCore<Color, Color, ColorOptions> DOColor1(
        this UIGradient gradient, Color color2, float duration) {
      return DOTween.To(gradient.GetColor1, gradient.SetColor1, color2, duration)
          .SetTarget(gradient);
    }

    public static TweenerCore<Color, Color, ColorOptions> DOColor2(
        this UIGradient gradient, Color color2, float duration) {
      return DOTween.To(gradient.GetColor2, gradient.SetColor2, color2, duration)
          .SetTarget(gradient);
    }

    public static TweenerCore<Color, Color, ColorOptions> DOBlendableColor1(
        this UIGradient gradient, Color color1, float duration) {
      Color endColor = color1 - gradient.color1;
      Color currentColor = Color.clear;

      Color GetColor() => currentColor;

      void SetColor(Color x) {
        Color diff = x - currentColor;
        currentColor = x;
        gradient.color1 += diff;
      }

      return DOTween.To(GetColor, SetColor, endColor, duration)
          .Blendable()
          .SetTarget(gradient);
    }

    public static TweenerCore<Color, Color, ColorOptions> DOBlendableColor2(
        this UIGradient gradient, Color color2, float duration) {
      Color endColor = color2 - gradient.color2;
      Color currentColor = Color.clear;

      Color GetColor() => currentColor;

      void SetColor(Color x) {
        Color diff = x - currentColor;
        currentColor = x;
        gradient.color2 += diff;
      }

      return DOTween.To(GetColor, SetColor, endColor, duration)
          .Blendable()
          .SetTarget(gradient);
    }

    public static Tweener DOPlayOneShot(this AudioSource target, AudioClip audioClip, float volumeScale) {
      float duration = audioClip.length;

      return DOVirtual
          .Float(0f, duration, duration, EmptyFloatCallback)
          .OnPlay(() => target.PlayOneShot(audioClip, volumeScale))
          .SetTarget(target);
    }

    static void EmptyFloatCallback(float x) {
      // ...
    }
  }
}
