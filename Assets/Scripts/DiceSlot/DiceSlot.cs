using UnityEngine;
using UnityEngine.EventSystems;

public class DiceSlot : MonoBehaviour, IDropHandler
{
  public int AtRow { get; private set; }
  public int AtCol { get; private set; }
  public Dice? Dice { get; private set; }
  public bool HasDice() => Dice != null;

  private RectTransform rectTransform;

  private void Awake()
  {
    rectTransform = GetComponent<RectTransform>();
  }

  public void Initialize(int row, int col, Dice? dice = null)
  {
    SetCoordinates(row, col);
    if (dice != null) PlaceDice(dice);
  }

  public void SetCoordinates(int row, int col)
  {
    AtRow = row;
    AtCol = col;
  }

  public void PlaceDice(Dice dice)
  {
    Dice = dice;
    Debug.Log($"Placed {Dice.Value} dice at Slot[{AtRow},{AtCol}].");
  }

  public void RemoveDice()
  {
    Dice = null;
    Debug.Log($"Cleared Slot[{AtRow},{AtCol}].");
  }

  public bool TryGetDice(out Dice? diceData)
  {
    diceData = Dice;
    return diceData != null;
  }

  public void OnDrop(PointerEventData eventData)
  {
    var diceGO = eventData.pointerDrag;
    if (!diceGO) return;

    var diceGOdata = diceGO.GetComponent<Dice>();
    PlaceDice(diceGOdata);

    var draggedRect = diceGO.GetComponent<RectTransform>();
    draggedRect.SetParent(rectTransform, worldPositionStays: false);
    draggedRect.anchoredPosition = Vector2.zero;
    draggedRect.localRotation = Quaternion.identity;
    draggedRect.localScale = Vector3.one;
  }
}