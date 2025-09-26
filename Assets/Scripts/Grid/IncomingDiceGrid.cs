using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class IncomingDiceGrid : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
  [SerializeField] private Vector2 _cellSize = new Vector2(100f, 100f);
  private Canvas _canvas;
  private RectTransform _rectTransform;
  private CanvasGroup _canvasGroup;
  private Vector2 _initialPosition;

  private BaseDice[,] _dices;
  public BaseDice[,] Dices => _dices;

  #region Unity Lifecycle
  private void Awake() {
    _rectTransform = GetComponent<RectTransform>();
    _canvasGroup = GetComponent<CanvasGroup>();

    _initialPosition = _rectTransform.anchoredPosition;

    if (_canvas == null)
      _canvas = GetComponentInParent<Canvas>();
    if (_canvas == null)
      _canvas = FindFirstObjectByType<Canvas>();
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.P)) {
      Renew();
    }

    if (Input.GetKeyDown(KeyCode.LeftArrow)) {
      Rotate(RotateDirection.Left);
    }

    if (Input.GetKeyDown(KeyCode.RightArrow)) {
      Rotate(RotateDirection.Right);
    }
  }
  #endregion

  #region Drag Handling
  public void OnBeginDrag(PointerEventData eventData) {
    _canvasGroup.blocksRaycasts = false;
    _canvasGroup.alpha = .6f;
  }

  public void OnDrag(PointerEventData eventData) {
    _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
  }

  public void OnEndDrag(PointerEventData eventData) {
    _canvasGroup.blocksRaycasts = true;
    _canvasGroup.alpha = 1f;
    _rectTransform.anchoredPosition = _initialPosition;
  }

  public void LockDrag() {
    enabled = false;
    _canvasGroup.blocksRaycasts = false;
  }

  public void UnlockDrag() {
    enabled = true;
    _canvasGroup.blocksRaycasts = true;
  }

  public void OnDropSucceful() {
    _rectTransform.anchoredPosition = _initialPosition;
    Renew();
  }
  #endregion

  #region Grid Management
  public void Init(BaseDice[,] initDices) {
    _dices = initDices;
    LayoutDiceGrid();
  }

  public void Renew() {
    int[,] template = IncomingDiceTemplate.RandomTemplate();
    int rows = template.GetLength(0);
    int cols = template.GetLength(1);

    int need = 0;
    for (int r = 0; r < rows; r++)
      for (int c = 0; c < cols; c++)
        if (template[r, c] != 0)
          need++;

    var allTypes = (DiceType[])Enum.GetValues(typeof(DiceType));
    int distinctTypes = allTypes.Length;

    if (need > distinctTypes) {
      Debug.LogWarning($"Renew(): Need {need} unique dice but only {distinctTypes} types exist. " + "Uniqueness will be enforced up to available types.");
    }

    var seenTypes = new HashSet<DiceType>();
    _dices = new BaseDice[rows, cols];

    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        if (template[r, c] == 0)
          continue;

        BaseDice randomDice = null;
        int attempts = 0;
        const int maxAttempts = 256;

        do {
          if (randomDice != null) {
            DiceSpawner.Instance.Release(randomDice);
            randomDice = null;
          }

          randomDice = DiceSpawner.Instance.SpawnRandom();
          attempts++;

          bool typeSeen = seenTypes.Contains(randomDice.Type);
          bool uniquenessPossible = seenTypes.Count < distinctTypes;

          if (!typeSeen || !uniquenessPossible) {
            break;
          }

        } while (attempts < maxAttempts);

        if (attempts >= maxAttempts) {
          Debug.LogWarning("Renew(): maxAttempts reached while trying to ensure unique types. " + "Accepting last spawned dice to avoid hang.");
        }

        _dices[r, c] = randomDice;
        seenTypes.Add(randomDice.Type);
      }
    }

    LayoutDiceGrid();
  }

  private void PlaceDice(int r, int c, BaseDice dice) {
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

  private void LayoutDiceGrid() {
    int rows = _dices.GetLength(0);
    int cols = _dices.GetLength(1);

    _rectTransform.sizeDelta = new Vector2(cols * _cellSize.x, rows * _cellSize.y);

    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        if (_dices[r, c] == null)
          continue;
        PlaceDice(r, c, _dices[r, c]);
      }
    }
  }
  #endregion

  #region Rotation
  private void Rotate(RotateDirection direction) {
    if (direction == RotateDirection.Left) {
      RotateLeft();
    }
    else {
      RotateRight();
    }
  }

  private void RotateRight() {
    int rows = _dices.GetLength(0);
    int cols = _dices.GetLength(1);

    BaseDice[,] rotated = new BaseDice[cols, rows];
    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        rotated[c, rows - r - 1] = _dices[r, c];
      }
    }

    _dices = rotated;
    LayoutDiceGrid();
  }

  private void RotateLeft() {
    int rows = _dices.GetLength(0);
    int cols = _dices.GetLength(1);

    BaseDice[,] rotated = new BaseDice[cols, rows];
    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        rotated[cols - 1 - c, r] = _dices[r, c];
      }
    }

    _dices = rotated;
    LayoutDiceGrid();
  }
  #endregion
}
