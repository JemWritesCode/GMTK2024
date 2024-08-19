using UnityEngine;
using UnityEngine.UI;

namespace YoloBox {
  public static class ImageExtensions {
    public static T SetSpriteIfValid<T>(this T image, Sprite sprite) where T : Image {
      if (sprite) {
        image.sprite = sprite;
      }

      return image;
    }
  }
}
