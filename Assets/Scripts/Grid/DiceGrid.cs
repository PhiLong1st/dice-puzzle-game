using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceGrid : MonoBehaviour, IGrid<BaseDice>, IDropHandler {
  public event Action OnDropSuccessful;

  [SerializeField] private Vector2 _cellSize = new Vector2(100f, 100f);
  [SerializeField] private Vector2 _spacing = new Vector2(10f, 10f);
  [SerializeField] private int _rows = 5;
  [SerializeField] private int _cols = 5;

  private RectTransform _rectTransform;
  private BaseDice[,] _dices;

  private IMergeResolver<BaseDice> _mergeResolver;
  private IPlacementHandler<BaseDice> _placementHandler;

  public BaseDice[,] Grid => _dices;
  public int Rows => _rows;
  public int Cols => _cols;
  public Vector2 CellSize => _cellSize;
  public Vector2 Spacing => _spacing;

  private void Awake() {
    _rectTransform = GetComponent<RectTransform>();
    _rectTransform.sizeDelta = new Vector2(_cols * _cellSize.x, _rows * _cellSize.y);

    _dices = new BaseDice[_rows, _cols];

    _mergeResolver = new MatchAndUpgradeMergeResolver();
    _placementHandler = new SimplePlacementHandler();
  }

  public void OnDrop(PointerEventData eventData) {
    GameObject draggedGO = eventData.pointerDrag;

    if (!draggedGO) {
      return;
    }

    (int row, int col) anchorCell = GetAnchorDropCell(draggedGO);
    var incomingDiceGrid = draggedGO.GetComponent<IncomingDiceGrid>();
    BaseDice[,] dices = incomingDiceGrid.Dices;

    ICollection<(int row, int col)> dropCells;

    if (!_placementHandler.TryPlace(this, dices, anchorCell, out dropCells)) {
      return;
    }

    incomingDiceGrid.OnDropSucceful();
    OnDropSuccessful?.Invoke();

    _mergeResolver.Resolve(this, dropCells);
  }

  public void Set(int row, int col, BaseDice dice) {
    dice.gameObject.name = $"{dice.Type}_{row}_{col}";

    _dices[row, col] = dice;

    RectTransform cellRectTransform = dice.GetComponent<RectTransform>();
    cellRectTransform.SetParent(_rectTransform, false);

    Vector2 tlCorner = _rectTransform.GetWorldCorner(CornerType.TopLeft);

    Vector2 cellPosition = new Vector2 {
      x = tlCorner.x + (col * _cellSize.x) + (_cellSize.x * cellRectTransform.pivot.x),
      y = tlCorner.y - ((row * _cellSize.y) + (_cellSize.y * cellRectTransform.pivot.y))
    };

    cellRectTransform.position = cellPosition;
  }

  public bool IsInBounds(int row, int col) => 0 <= row && row < _rows && 0 <= col && col < _cols;

  public bool IsEmpty(int row, int col) => _dices[row, col] == null;

  public void Clear(int row, int col) {
    _dices[row, col].Release();
    _dices[row, col] = null;
  }

  public BaseDice Get(int row, int col) => _dices[row, col];

  private (int row, int col) GetAnchorDropCell(GameObject draggedGO) {
    Vector2 gridTopLeftWorld = _rectTransform.GetWorldCorner(CornerType.TopLeft);
    Vector2 dropTopLeftWorld = draggedGO.GetComponent<RectTransform>().GetWorldCorner(CornerType.TopLeft);
    Vector2 anchorCellCenterWorld = dropTopLeftWorld + new Vector2(_cellSize.x * 0.5f, -_cellSize.y * 0.5f);

    int tlDropRow = (int)(Mathf.Abs(gridTopLeftWorld.y - anchorCellCenterWorld.y) / (int)_cellSize.y);
    int tlDropCol = (int)(Mathf.Abs(gridTopLeftWorld.x - anchorCellCenterWorld.x) / (int)_cellSize.x);

    return (tlDropRow, tlDropCol);
  }
}
