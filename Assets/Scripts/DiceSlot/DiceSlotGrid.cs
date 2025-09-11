using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceSlotGrid : MonoBehaviour, IDropHandler
{
  [SerializeField] public int rows;
  [SerializeField] public int cols;
  public int Rows => rows;
  public int Cols => cols;
  private DiceSlot[,] diceSlotGrid;

  [SerializeField] private GameObject diceSlotPrefab;
  private RectTransform rectTransform;

  private void Start()
  {
    rectTransform = GetComponent<RectTransform>();
    BuildGrid();
  }

  private void BuildGrid()
  {
    if (!diceSlotPrefab) return;

    diceSlotGrid = new DiceSlot[rows, cols];
    for (int r = 0; r < rows; ++r)
    {
      for (int c = 0; c < cols; ++c)
      {
        var clonedDiceSlot = Instantiate(diceSlotPrefab);
        clonedDiceSlot.gameObject.name += $"_{r}_{c}";
        diceSlotGrid[r, c] = clonedDiceSlot.GetComponent<DiceSlot>();
        diceSlotGrid[r, c].SetCoordinates(r, c);
        if (r == 2 && c == 2)
        {
          diceSlotGrid[r, c].PlaceDice(DiceManager.Instance.GetDicePrefab(DiceType.Six).GetComponent<Dice>());
        }

        var goRect = diceSlotGrid[r, c].GetComponent<RectTransform>();
        goRect.SetParent(rectTransform, worldPositionStays: false);
      }
    }
  }

  public void OnDrop(PointerEventData eventData)
  {
    var diceGO = eventData.pointerDrag;
    if (!diceGO) return;

    var diceInput = diceGO.GetComponent<IncomingDice>();
    var dragRect = diceGO.GetComponent<RectTransform>();
    var isDropOk = TryDrop(rectTransform, dragRect, out Vector2 anchoredDropPos, out (int row, int col) nearestCell);
    if (!isDropOk) return;
    diceInput.NotifyDropSuccessful();
    dragRect.anchoredPosition = anchoredDropPos;
    ApplyInputDrop(nearestCell, dragRect);
  }

  private void ApplyInputDrop((int row, int col) nearestCell, RectTransform dragRect)
  {
    Dice?[,] incomingDices = dragRect.gameObject.GetComponent<IncomingDice>().IncomingDices;
    int inputRows = incomingDices.GetLength(0);
    int inputCols = incomingDices.GetLength(1);

    for (int r = nearestCell.row; r < nearestCell.row + inputRows; ++r)
    {
      for (int c = nearestCell.col; c < nearestCell.col + inputCols; ++c)
      {
        Dice? diceInput = incomingDices[r - nearestCell.row, c - nearestCell.col];
        if (diceInput == null) continue;

        DiceSlot diceSlot = diceSlotGrid[r, c];
        diceSlot.RemoveDice();
        diceSlot.PlaceDice(diceInput);
        Destroy(diceInput);
      }
    }
  }

  private bool TryDrop(RectTransform gridRect, RectTransform dragRect, out Vector2 anchoredDropPos, out (int row, int col) nearestCell)
  {
    anchoredDropPos = default;
    nearestCell = (-1, -1);

    Vector2 gridTopLeft = gridRect.TopLeftCorner();
    Vector2 gridBottomRight = gridRect.BottomRightCorner();

    Vector2 dragTopLeft = dragRect.TopLeftCorner();
    Vector2 dragTopRight = dragRect.TopRightCorner();
    Vector2 dragBottomLeft = dragRect.BottomLeftCorner();
    Vector2 dragBottomRight = dragRect.BottomRightCorner();

    bool Inside(Vector2 p) =>
        gridTopLeft.x <= p.x && p.x <= gridBottomRight.x &&
        gridBottomRight.y <= p.y && p.y <= gridTopLeft.y;

    bool isFullyInsideBounds = Inside(dragTopLeft) && Inside(dragTopRight)
                            && Inside(dragBottomLeft) && Inside(dragBottomRight);

    if (!isFullyInsideBounds)
    {
      Debug.Log("Fail because drop outside grid");
      return false;
    }

    float minimumDistance = float.MaxValue;
    for (int r = 0; r < rows; ++r)
    {
      for (int c = 0; c < cols; ++c)
      {
        RectTransform slotRect = diceSlotGrid[r, c].GetComponent<RectTransform>();
        Vector2 slotTopLeft = gridTopLeft + slotRect.TopLeftCorner();
        float distance = (dragTopLeft - slotTopLeft).magnitude;

        if (minimumDistance > distance)
        {
          minimumDistance = distance;
          anchoredDropPos = slotTopLeft;
          nearestCell = (r, c);
        }
      }
    }

    Dice[,] incomingDices = dragRect.gameObject.GetComponent<IncomingDice>().IncomingDices;
    int inputRows = incomingDices.GetLength(0);
    int inputCols = incomingDices.GetLength(1);

    for (int r = nearestCell.row; r < nearestCell.row + inputRows; ++r)
    {
      for (int c = nearestCell.col; c < nearestCell.col + inputCols; ++c)
      {
        bool isDiceOverlaps = incomingDices[r - nearestCell.row, c - nearestCell.col] != null && diceSlotGrid[r, c].Dice != null;
        if (isDiceOverlaps)
        {
          Debug.Log("Fail because drop overlap other dice in grid");
          return false;
        }
      }
    }

    anchoredDropPos.x += dragRect.pivot.x * dragRect.sizeDelta.x;
    anchoredDropPos.y -= dragRect.pivot.y * dragRect.sizeDelta.y;
    return true;
  }
}