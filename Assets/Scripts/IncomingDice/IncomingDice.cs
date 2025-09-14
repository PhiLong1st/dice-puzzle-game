using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class IncomingDice : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
  public Dice?[,] IncomingDices { get; private set; }

  private Canvas canvas;
  private RectTransform rectTransform;
  private CanvasGroup canvasGroup;

  public bool IsDropSuccessful { get; private set; }
  public event Action OnDropSuccessful;

  private Vector2 initialPosition;
  private IncomingDiceVisual visual;

  private void Awake() {
    visual = GetComponent<IncomingDiceVisual>();
    rectTransform = GetComponent<RectTransform>();
    canvasGroup = GetComponent<CanvasGroup>();

    if (canvas == null)
      canvas = GetComponentInParent<Canvas>();
    if (canvas == null)
      canvas = FindFirstObjectByType<Canvas>();
    if (canvas == null)
      Debug.LogError("No Canvas found for DiceInput.", this);
  }

  private void Start() {
    IsDropSuccessful = false;
    enabled = true;
    canvasGroup.blocksRaycasts = true;
  }

  public void Initialze(DiceType?[,] diceTypes) {
    int rows = diceTypes.GetLength(0);
    int cols = diceTypes.GetLength(1);

    IncomingDices = new Dice?[rows, cols];
    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        if (diceTypes[r, c] == null) {
          RectTransform emptyPrefabRect = DiceManager.Instance.EmptyDicePrefab.GetComponent<RectTransform>();
          emptyPrefabRect.SetParent(rectTransform, worldPositionStays: false);
          continue;
        }

        GameObject dicePrefab = DiceManager.Instance.GetDicePrefab(diceTypes[r, c].Value);
        IncomingDices[r, c] = dicePrefab.GetComponent<Dice>();

        RectTransform prefabRect = dicePrefab.GetComponent<RectTransform>();
        prefabRect.SetParent(rectTransform, worldPositionStays: false);
      }
    }

    visual.Initilize();
  }

  public void OnBeginDrag(PointerEventData eventData) {
    initialPosition = rectTransform.anchoredPosition;
    canvasGroup.blocksRaycasts = false;
    canvasGroup.alpha = .6f;
  }

  public void OnDrag(PointerEventData eventData) {
    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
  }

  public void OnEndDrag(PointerEventData eventData) {
    canvasGroup.blocksRaycasts = true;
    canvasGroup.alpha = 1f;

    if (IsDropSuccessful) {
      Debug.Log("Drop succesfull");
      OnDropSuccessful?.Invoke();
      Destroy(gameObject);
    }
    else {
      Debug.Log("Drop fail so comeback the initial position");
      rectTransform.anchoredPosition = initialPosition;
    }
  }

  public void NotifyDropSuccessful() {
    IsDropSuccessful = true;
  }

  public void LockForDrag() {
    enabled = false;
    canvasGroup.blocksRaycasts = false;
  }

  public void UnlockForDrag() {
    enabled = true;
    canvasGroup.blocksRaycasts = true;
  }

  public void RotateRight() {
    int rows = IncomingDices.GetLength(0);
    int cols = IncomingDices.GetLength(1);

    Dice?[,] rotated = new Dice?[cols, rows];
    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        rotated[c, rows - r - 1] = IncomingDices[r, c];
      }
    }

    IncomingDices = rotated;
    visual.RotateRight();
  }

  public void RotateLeft() {
    int rows = IncomingDices.GetLength(0);
    int cols = IncomingDices.GetLength(1);

    Dice?[,] rotated = new Dice?[cols, rows];
    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        rotated[cols - 1 - c, r] = IncomingDices[r, c];
      }
    }

    IncomingDices = rotated;
    visual.RotateLeft();
  }
}
