using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceGrid : BaseGrid, IDropHandler {
  public event Action OnDropSuccessful;

  public void OnDrop(PointerEventData eventData) {
    var draggedGO = eventData.pointerDrag;

    if (!draggedGO)
      return;

    Vector2 gridTopLeftWorld = rectTransform.GetWorldCorner(CornerType.TopLeft);
    Vector2 dropTopLeftWorld = draggedGO.GetComponent<RectTransform>().GetWorldCorner(CornerType.TopLeft);
    Vector2 anchorCellCenterWorld = dropTopLeftWorld + new Vector2(cellSize.x * 0.5f, -cellSize.y * 0.5f);

    int dropRow = (int)(Mathf.Abs(gridTopLeftWorld.y - anchorCellCenterWorld.y) / (int)cellSize.y);
    int dropCol = (int)(Mathf.Abs(gridTopLeftWorld.x - anchorCellCenterWorld.x) / (int)cellSize.x);
    Debug.Log($"Start drop at [{dropRow}, {dropCol}]");

    var incomingDice = draggedGO.GetComponent<IncomingDice>();
    Dice?[,] incomingDices = draggedGO.GetComponent<IncomingDice>().Dices;

    for (int r = 0; r < incomingDices.GetLength(0); ++r) {
      for (int c = 0; c < incomingDices.GetLength(1); ++c) {
        if (incomingDices[r, c] == null)
          continue;

        bool isInBounds = IsInBounds(r + dropRow, c + dropCol);
        if (!isInBounds) {
          return;
        }

        bool isDiceOverlaps = Dices[r + dropRow, c + dropCol] != null;
        if (isDiceOverlaps) {
          return;
        }
      }
    }

    for (int r = 0; r < incomingDices.GetLength(0); ++r) {
      for (int c = 0; c < incomingDices.GetLength(1); ++c) {
        if (incomingDices[r, c] == null)
          continue;

        PlaceDice(incomingDices[r, c], r + dropRow, c + dropCol);
      }
    }

    incomingDice.NotifyDropSuccessful();
    OnDropSuccessful?.Invoke();
  }
}

