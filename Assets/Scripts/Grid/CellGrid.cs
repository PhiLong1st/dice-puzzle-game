using UnityEngine;
public class CellGrid : MonoBehaviour {
  public int AtRow { get; private set; }
  public int AtCol { get; private set; }

  private RectTransform rectTransform;

  private void Awake() {
    rectTransform = GetComponent<RectTransform>();
  }

  public void SetCoordinates(int row, int col) {
    AtRow = row;
    AtCol = col;
  }

  public void PlaceChild(Dice dice) {
    RectTransform childRect = dice.GetComponent<RectTransform>();
    childRect.SetAsChild(rectTransform);
    childRect.ApplyUiLayout(UiLayoutMode.Center);
  }

  public void ClearChild() {
    for (int i = transform.childCount - 1; i >= 0; i--)
      Destroy(transform.GetChild(i).gameObject);
  }
}
