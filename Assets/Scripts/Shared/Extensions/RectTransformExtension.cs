using UnityEngine;

public static class RectTransformExtensions {
  public static Vector3 GetWorldCorner(this RectTransform rt, CornerType cornerType) {
    Vector3[] worldCorners = new Vector3[4];
    rt.GetWorldCorners(worldCorners);
    return worldCorners[(int)cornerType];
  }
}
