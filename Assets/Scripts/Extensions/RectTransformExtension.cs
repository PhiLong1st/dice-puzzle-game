using UnityEngine;

public class RectCornerLocator {
  public enum Horizontal { Left, Right }
  public enum Vertical { Down, Up }

  private static int Sign(Horizontal d) => d == Horizontal.Left ? -1 : +1;
  private static int Sign(Vertical d) => d == Vertical.Down ? -1 : +1;

  public static Vector2 GetCornerPosition(RectTransform rt, Horizontal x, Vertical y) {
    Vector2 p = rt.anchoredPosition;
    p.x += Sign(x) * rt.pivot.x * rt.sizeDelta.x;
    p.y += Sign(y) * rt.pivot.y * rt.sizeDelta.y;
    return p;
  }
}
public enum CornerType { BottomLeft = 0, TopLeft = 1, TopRight = 2, BottomRight = 3 }

public enum UiLayoutMode { Keep, Center, Stretch }

public static class RectTransformExtensions {
  public static Vector3 GetWorldCorner(this RectTransform rt, CornerType cornerType) {
    Vector3[] worldCorners = new Vector3[4];
    rt.GetWorldCorners(worldCorners);
    return worldCorners[(int)cornerType];
  }

  public static Vector2 GetCorner(this RectTransform rt, RectCornerLocator.Horizontal x, RectCornerLocator.Vertical y)
      => RectCornerLocator.GetCornerPosition(rt, x, y);

  public static Vector2 TopLeftCorner(this RectTransform rt)
      => rt.GetCorner(RectCornerLocator.Horizontal.Left, RectCornerLocator.Vertical.Up);

  public static Vector2 TopRightCorner(this RectTransform rt)
      => rt.GetCorner(RectCornerLocator.Horizontal.Right, RectCornerLocator.Vertical.Up);

  public static Vector2 BottomLeftCorner(this RectTransform rt)
      => rt.GetCorner(RectCornerLocator.Horizontal.Left, RectCornerLocator.Vertical.Down);

  public static Vector2 BottomRightCorner(this RectTransform rt)
      => rt.GetCorner(RectCornerLocator.Horizontal.Right, RectCornerLocator.Vertical.Down);

  public static void ApplyUiLayout(this RectTransform rt, UiLayoutMode mode) {
    switch (mode) {
      case UiLayoutMode.Center:
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        break;

      case UiLayoutMode.Stretch:
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
        break;

      case UiLayoutMode.Keep:
        break;

      default:
        break;
    }
  }

  public static void SetAsChild(this RectTransform rt, RectTransform parentRect) {
    rt.SetParent(parentRect, worldPositionStays: false);
  }
}
