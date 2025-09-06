using UnityEngine;
public class DiceSlot : MonoBehaviour
{
  public int AtRow { get; private set; }
  public int AtCol { get; private set; }
  public Dice Dice { get; private set; }
  public bool HasDice() => Dice != null;

  private RectTransform rectTransform;

  private void Awake()
  {
    rectTransform = GetComponent<RectTransform>();
  }

  public void SetCoordinates(int row, int col)
  {
    AtRow = row;
    AtCol = col;
  }

  public void PlaceDice(Dice dice)
  {
    Dice = dice;
    RectTransform diceRectTransform = Instantiate(Dice.gameObject).GetComponent<RectTransform>();
    diceRectTransform.SetParent(rectTransform, worldPositionStays: false);
    Debug.Log($"Placed {Dice.Type} dice at slot[{AtRow},{AtCol}].");
  }

  public void RemoveDice()
  {
    Debug.Log($"Cleared Slot[{AtRow},{AtCol}].");
  }
}