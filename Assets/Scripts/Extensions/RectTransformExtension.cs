using Unity.VisualScripting;
using UnityEngine;

public class RectCornerLocator
{
  public enum Horizontal { Left, Right }
  public enum Vertical { Down, Up }

  private static int Sign(Horizontal d) => d == Horizontal.Left ? -1 : +1;
  private static int Sign(Vertical d) => d == Vertical.Down ? -1 : +1;

  public static Vector2 GetCornerPosition(RectTransform rt, Horizontal x, Vertical y)
  {
    Vector2 p = rt.anchoredPosition;
    p.x += Sign(x) * rt.pivot.x * rt.sizeDelta.x;
    p.y += Sign(y) * rt.pivot.y * rt.sizeDelta.y;
    return p;
  }
}

public static class RectTransformExtensions
{
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
}