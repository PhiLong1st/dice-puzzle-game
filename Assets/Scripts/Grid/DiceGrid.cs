using System;
using System.Collections.Generic;
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
    GameObject draggedGO = eventData.pointerDrag;

    if (!CanDrop(draggedGO)) {
      return;
    }

    (int row, int col) anchorCell = GetAnchorDropCell(draggedGO);
    var incomingDiceGrid = draggedGO.GetComponent<IncomingDiceGrid>();
    BaseDice[,] incomingDices = incomingDiceGrid.Dices;

    for (int r = 0; r < incomingDices.GetLength(0); ++r) {
      for (int c = 0; c < incomingDices.GetLength(1); ++c) {
        if (!incomingDices[r, c])
          continue;

        PlaceDice(r + anchorCell.row, c + anchorCell.col, incomingDices[r, c]);
      }
    }

    incomingDiceGrid.OnDropSucceful();
    OnDropSuccessful?.Invoke();
  }

  public void HandleAfterDrop(List<(int row, int col)> incomingDiceCells) {
    int rows = _dices.GetLength(0);
    int cols = _dices.GetLength(1);

    int[] dx = { -1, 0, 0, 1 };
    int[] dy = { 0, -1, 1, 0 };
    bool[,] vst = new bool[rows, cols];

    Stack<(int, int)> st = new();

    void DFS(int r, int c) {
      Debug.Log($"DFS: [{r}, {c}]");
      vst[r, c] = true;
      st.Push((r, c));
      for (int i = 0; i < 4; ++i) {
        int newRow = r + dx[i];
        int newCol = c + dy[i];

        if (!IsInBounds(newRow, newCol)) {
          Debug.Log($"DFS: [{r} {c}] - IsInBounds");
          continue;
        }

        if (vst[newRow, newCol]) {
          Debug.Log($"DFS: [{r} {c}] - (vst[newRow, newCol])");
          continue;
        }

        if (_dices[newRow, newCol] == null) {
          Debug.Log($"DFS: [{r} {c}] - (_dices[newRow, newCol] == null)");
          continue;
        }

        if (_dices[newRow, newCol].Type != _dices[r, c].Type) {
          Debug.Log($"DFS: [{r} {c}] - dices[newRow, newCol].Type != dices[r, c].Type");
          continue;
        }

        DFS(newRow, newCol);
      }
    }

    foreach (var cell in incomingDiceCells) {
      Debug.Log($"Start at [{cell.row}, {cell.col}]");

      if (vst[cell.row, cell.col])
        continue;

      DiceType targetType = _dices[cell.row, cell.col].Type;
      DiceType? nextDiceType = _dices[cell.row, cell.col].GetNextDiceType();
      DFS(cell.row, cell.col);

      if (st.Count >= 3) {
        Debug.Log($"========= Start with type {targetType} at [{cell.row}, {cell.col}]=========");

        while (st.Count > 0) {
          (int r, int c) = st.Peek();
          Debug.Log($"[{r}, {c}] -> {_dices[r, c]} -> Point: {_dices[r, c].Point}");
          // GameData.AddScore(_dices[r, c].Value);
          DiceSpawner.Instance.Release(_dices[cell.row, cell.col]);
          st.Pop();
        }

        var nextDice = DiceSpawner.Instance.SpawnByType(targetType);
        if (nextDice != null) {
          PlaceDice(cell.row, cell.col, nextDice);
        }

        Debug.Log($"========= End =========");
      }

      st.Clear();
    }
  }

  private void PlaceDice(int r, int c, BaseDice dice) {
    dice.gameObject.name = $"{dice.Type}_{r}_{c}";

    _dices[r, c] = dice;

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

  private bool IsCellEmpty(int r, int c) => _dices[r, c] == null;

  private bool CanDrop(GameObject draggedGO) {
    if (!draggedGO) {
      return false;
    }

    (int row, int col) anchorCell = GetAnchorDropCell(draggedGO);
    var incomingDiceGrid = draggedGO.GetComponent<IncomingDiceGrid>();
    BaseDice[,] incomingDices = incomingDiceGrid.Dices;

    for (int r = 0; r < incomingDices.GetLength(0); ++r) {
      for (int c = 0; c < incomingDices.GetLength(1); ++c) {
        if (!incomingDices[r, c]) {
          continue;
        }

        int dropRow = r + anchorCell.row;
        int dropCol = c + anchorCell.col;
        if (!IsInBounds(dropRow, dropCol)) {
          return false;
        }

        if (!IsCellEmpty(dropRow, dropCol)) {
          return false;
        }
      }
    }

    return true;
  }

  private (int row, int col) GetAnchorDropCell(GameObject draggedGO) {
    Vector2 gridTopLeftWorld = _rectTransform.GetWorldCorner(CornerType.TopLeft);
    Vector2 dropTopLeftWorld = draggedGO.GetComponent<RectTransform>().GetWorldCorner(CornerType.TopLeft);
    Vector2 anchorCellCenterWorld = dropTopLeftWorld + new Vector2(_cellSize.x * 0.5f, -_cellSize.y * 0.5f);

    int tlDropRow = (int)(Mathf.Abs(gridTopLeftWorld.y - anchorCellCenterWorld.y) / (int)_cellSize.y);
    int tlDropCol = (int)(Mathf.Abs(gridTopLeftWorld.x - anchorCellCenterWorld.x) / (int)_cellSize.x);

    return (tlDropRow, tlDropCol);
  }
}
