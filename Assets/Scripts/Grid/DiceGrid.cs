using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceGrid : MonoBehaviour, IDropHandler {
  public event Action OnDropSuccessful;

  [SerializeField] private Vector2 _cellSize = new Vector2(100f, 100f);
  [SerializeField] private int _rows = 5;
  [SerializeField] private int _cols = 5;

  private RectTransform _rectTransform;
  private BaseDice[,] _dices;

  private void Awake() {
    _rectTransform = GetComponent<RectTransform>();

    _dices = new BaseDice[_rows, _cols];
    _rectTransform.sizeDelta = new Vector2(_cols * _cellSize.x, _rows * _cellSize.y);
  }

  public void OnDrop(PointerEventData eventData) {
    var draggedGO = eventData.pointerDrag;

    if (!draggedGO)
      return;

    Vector2 gridTopLeftWorld = _rectTransform.GetWorldCorner(CornerType.TopLeft);
    Vector2 dropTopLeftWorld = draggedGO.GetComponent<RectTransform>().GetWorldCorner(CornerType.TopLeft);
    Vector2 anchorCellCenterWorld = dropTopLeftWorld + new Vector2(_cellSize.x * 0.5f, -_cellSize.y * 0.5f);

    int dropRow = (int)(Mathf.Abs(gridTopLeftWorld.y - anchorCellCenterWorld.y) / (int)_cellSize.y);
    int dropCol = (int)(Mathf.Abs(gridTopLeftWorld.x - anchorCellCenterWorld.x) / (int)_cellSize.x);

    var incomingDiceGrid = draggedGO.GetComponent<IncomingDiceGrid>();
    BaseDice[,] dices = incomingDiceGrid.Dices;

    for (int r = 0; r < dices.GetLength(0); ++r) {
      for (int c = 0; c < dices.GetLength(1); ++c) {
        if (!dices[r, c])
          continue;

        bool isInBounds = IsInBounds(r + dropRow, c + dropCol);
        if (!isInBounds) {
          return;
        }

        if (_dices[r + dropRow, c + dropCol]) {
          return;
        }
      }
    }

    for (int r = 0; r < dices.GetLength(0); ++r) {
      for (int c = 0; c < dices.GetLength(1); ++c) {
        if (!dices[r, c])
          continue;

        PlaceDice(r + dropRow, c + dropCol, dices[r, c]);
      }
    }
    incomingDiceGrid.OnDropSucceful();
    OnDropSuccessful?.Invoke();
  }

  private void PlaceDice(int r, int c, BaseDice dice) {
    RectTransform cellRectTransform = dice.GetComponent<RectTransform>();
    cellRectTransform.SetParent(_rectTransform, false);

    Vector2 tlCorner = _rectTransform.GetWorldCorner(CornerType.TopLeft);

    Vector2 cellPosition = new Vector2 {
      x = tlCorner.x + (c * _cellSize.x) + (_cellSize.x * cellRectTransform.pivot.x),
      y = tlCorner.y - ((r * _cellSize.y) + (_cellSize.y * cellRectTransform.pivot.y))
    };

    cellRectTransform.position = cellPosition;
  }

  private bool IsInBounds(int r, int c) => 0 <= r && r < _rows && 0 <= c && c < _cols;
}
