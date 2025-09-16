using UnityEngine;
using UnityEngine.UI;

public abstract class BaseGrid : MonoBehaviour {
  public int Rows => rows;
  public int Cols => cols;
  public Color32 BackgroundColor => backgroundColor;
  public Vector2 CellSize => cellSize;
  public Vector2 GridPixelSize {
    get {
      float w = cols * cellSize.x;
      float h = rows * cellSize.y;
      return new Vector2(w, h);
    }
  }

  protected GameObject visualGO { get; private set; }
  protected GameObject containerGO { get; private set; }
  protected const int MinDim = 1;
  protected const int MaxDim = 10;
  protected const float MinCellSize = 1f;
  protected const float MinSpacing = 0f;
  protected Dice?[,] Dices { get; private set; }


  [Header("Grid")]
  [SerializeField] protected Color32 backgroundColor = new Color32(50, 10, 20, 105);
  [SerializeField, Min(MinDim)] protected int rows = 5;
  [SerializeField, Min(MinDim)] protected int cols = 5;

  [Header("Cell")]
  [SerializeField] protected Vector2 cellSize = new Vector2(100f, 100f);
  protected CellGrid[,] Cells;

  [SerializeField] protected GameObject cellGridPrefab;
  protected GridLayoutGroup gridLayoutGroup;
  protected Image bgImage;
  protected RectTransform rectTransform;

  #region MonoBehaviour Unity Lifecycle
  private void Awake() {
    visualGO = new GameObject("Visual");
    containerGO = new GameObject("Container");

    rectTransform = GetComponent<RectTransform>();

    var visualRect = visualGO.AddComponent<RectTransform>();
    visualRect.SetAsChild(rectTransform);
    visualRect.ApplyUiLayout(UiLayoutMode.Stretch);

    var containerRect = containerGO.AddComponent<RectTransform>();
    containerRect.SetAsChild(rectTransform);
    containerRect.ApplyUiLayout(UiLayoutMode.Stretch);

    gridLayoutGroup = containerGO.AddComponent<GridLayoutGroup>();

    bgImage = visualGO.AddComponent<Image>();

    OnAwake();
  }

  protected void Start() {
    if (!TryValidate(out var err)) {
      Debug.LogError(err, this);
      return;
    }

    ApplySettings();
    BuildGrid();
  }
  #endregion

  #region Virtual Functions
  protected virtual void OnAwake() { }
  #endregion

  #region Grid Functions
  protected void InitializeGrid(DiceType?[,] diceTypes) {
    int initGridRows = diceTypes.GetLength(0);
    int initGridCols = diceTypes.GetLength(1);

    if (initGridRows != rows || initGridCols != cols) {
      Debug.LogError("Data grid is not the same dimension!");
      return;
    }

    for (int r = 0; r < initGridRows; ++r) {
      for (int c = 0; c < initGridCols; ++c) {
        if (!diceTypes[r, c].HasValue) {
          continue;
        }

        var diceType = diceTypes[r, c].Value;
        if (!DiceManager.Instance.TryGetDice(diceType, out Dice? dice) || dice == null) {
          Debug.LogError($"Failed to get dice prefab for {diceType} at ({r},{c}).", this);
          continue;
        }

        PlaceDice(dice, r, c);
      }
    }
  }

  protected void PlaceDice(Dice dice, int r, int c) {
    Dices[r, c] = dice;
    Cells[r, c].PlaceChild(dice);
  }

  protected void RemoveDice(int r, int c) {
    if (Dices[r, c] == null) {
      Debug.Log("Cell is already empty!");
      return;
    }

    Dices[r, c] = null;
    Cells[r, c].ClearChild();
  }

  #endregion

  #region Setting Functions
  private void BuildGrid() {
    if (!cellGridPrefab)
      return;

    Cells = new CellGrid[rows, cols];
    Dices = new Dice?[rows, cols];
    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        GameObject cellGO = Instantiate(cellGridPrefab);
        cellGO.name += $"_{r}_{c}";

        Cells[r, c] = cellGO.GetComponent<CellGrid>();
        Cells[r, c].SetCoordinates(r, c);

        RectTransform containerRect = containerGO.GetComponent<RectTransform>();
        RectTransform goRect = Cells[r, c].GetComponent<RectTransform>();
        goRect.SetAsChild(containerRect);
      }
    }
  }

  private void ApplySettings() {
    rectTransform.sizeDelta = GridPixelSize;

    if (gridLayoutGroup != null) {
      gridLayoutGroup.cellSize = cellSize;
      gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
      gridLayoutGroup.childAlignment = TextAnchor.UpperLeft;
      gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
      gridLayoutGroup.constraintCount = cols;
    }

    if (bgImage != null) {
      bgImage.color = backgroundColor;
    }
  }

  private void Sanitize(bool logWarnings = false) {
    int oldRows = rows;
    int oldCols = cols;

    rows = Mathf.Clamp(rows, MinDim, MaxDim);
    cols = Mathf.Clamp(cols, MinDim, MaxDim);
    cellSize = new Vector2(Mathf.Max(MinCellSize, cellSize.x), Mathf.Max(MinCellSize, cellSize.y));

    if (!logWarnings)
      return;

    if (rows != oldRows || cols != oldCols) {
      Debug.LogWarning($"GridSetting: clamped grid to {rows}x{cols} (allowed {MinDim}..{MaxDim}).", this);
    }

    if (cellSize.x <= MinCellSize || cellSize.y <= MinCellSize) {
      Debug.LogWarning($"GridSetting: cellSize must be > {MinCellSize} each axis. Now {cellSize}.", this);
    }
  }

  private bool TryValidate(out string message) {
    Sanitize(logWarnings: false);

    if (rows < MinDim || cols < MinDim) {
      message = $"Rows/Cols must be ≥ {MinDim}. Got {rows}x{cols}.";
      return false;
    }

    if (cellSize.x <= MinCellSize || cellSize.y <= MinCellSize) {
      message = $"cellSize must be > {MinCellSize} on both axes. Got {cellSize}.";
      return false;
    }

    message = string.Empty;
    return true;
  }
  #endregion

  #region Utility
  protected bool IsInBounds(int r, int c) => 0 <= r && r < Rows && 0 <= c && c < Cols;
  #endregion
}
