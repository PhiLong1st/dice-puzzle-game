using UnityEngine;
public class DiceSlot : MonoBehaviour {
  public int AtRow { get; private set; }
  public int AtCol { get; private set; }
  public Dice? Dice { get; private set; }
  public bool HasDice() => Dice != null;

  private RectTransform rectTransform;

  private void Awake() {
    rectTransform = GetComponent<RectTransform>();
  }

  public void SetCoordinates(int row, int col) {
    AtRow = row;
    AtCol = col;
  }

  public void PlaceDice(Dice dice) {
    Dice = dice;
    RectTransform diceRectTransform = dice.GetComponent<RectTransform>();
    diceRectTransform.SetParent(rectTransform, worldPositionStays: false);
    diceRectTransform.anchoredPosition = Vector2.zero;
    diceRectTransform.pivot = new Vector2(0.5f, 0.5f);
    diceRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
    diceRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
  }

  public void RemoveDice() {
    if (!HasDice()) {
      return;
    }

    Destroy(Dice.gameObject);
  }
}
