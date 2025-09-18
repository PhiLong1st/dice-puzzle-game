using UnityEngine;
using UnityEngine.UI;

public abstract class BaseGrid : MonoBehaviour {
  public int Rows => rows;
  public int Cols => cols;
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

  public Dice?[,] Dices { get; private set; }
  protected DiceType?[,] diceTypes { get; private set; }
  protected CellGrid[,] cellGrids { get; private set; }

  [Header("Grid")]
  [SerializeField] protected Color32 backgroundColor = new Color32(50, 10, 20, 105);
  [SerializeField, Min(MinDim)] protected int rows = 5;
  [SerializeField, Min(MinDim)] protected int cols = 5;

  [Header("Cell")]
  [SerializeField] protected Vector2 cellSize = new Vector2(100f, 100f);
  [SerializeField] private GameObject cellGridPrefab;

  protected GridLayoutGroup gridLayoutGroup;
  protected Image bgImage;
  protected RectTransform rectTransform;

  #region MonoBehaviour Unity Lifecycle
  private void Awake() {
    rectTransform = GetComponent<RectTransform>();

    visualGO = new GameObject("Visual");
    var visualRect = visualGO.AddComponent<RectTransform>();
    visualRect.SetAsChild(rectTransform);
    visualRect.ApplyUiLayout(UiLayoutMode.Stretch);

    containerGO = new GameObject("Container");
    var containerRect = containerGO.AddComponent<RectTransform>();
    containerRect.SetAsChild(rectTransform);
    containerRect.ApplyUiLayout(UiLayoutMode.Stretch);

    gridLayoutGroup = containerGO.AddComponent<GridLayoutGroup>();

    bgImage = visualGO.AddComponent<Image>();

    OnAwake();
  }

  private void Start() {
    if (!TryValidate(out var err)) {
      Debug.LogError(err, this);
      return;
    }

    ApplySettings();
    InitGrid();
    BuildGrid();
  }
  #endregion

  #region Virtual Functions
  protected virtual void OnAwake() { }
  #endregion

  #region Grid Functions
  public void SetGridData(DiceType?[,] data) {
    if (data == null) {
      return;
    }

    diceTypes = data;
  }

  protected void PlaceDice(Dice dice, int r, int c) {
    diceTypes[r, c] = dice.Type;
    Dices[r, c] = dice;
    cellGrids[r, c].PlaceChild(dice);
  }

  protected void RemoveDice(int r, int c) {
    diceTypes[r, c] = null;
    Dices[r, c] = null;
    cellGrids[r, c].ClearChild();
  }

  private void InitGrid() {
    if (!cellGridPrefab)
      return;

    Dices ??= new Dice?[rows, cols];
    cellGrids ??= new CellGrid[rows, cols];
    diceTypes ??= new DiceType?[rows, cols];

    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        GameObject cellGO = Instantiate(cellGridPrefab);
        cellGO.name += $"_{r}_{c}";

        cellGrids[r, c] = cellGO.GetComponent<CellGrid>();
        cellGrids[r, c].SetCoordinates(r, c);

        RectTransform containerRect = containerGO.GetComponent<RectTransform>();
        RectTransform goRect = cellGrids[r, c].GetComponent<RectTransform>();
        goRect.SetAsChild(containerRect);
      }
    }
  }

  private void BuildGrid() {
    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        if (diceTypes[r, c] == null)
          continue;

        Dice dice = DiceManager.Instance.GetDicePrefab(diceTypes[r, c].Value).GetComponent<Dice>();
        PlaceDice(dice, r, c);
      }
    }
  }
  #endregion

  #region Setting Functions
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
