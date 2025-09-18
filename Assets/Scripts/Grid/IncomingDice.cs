using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class IncomingDice : BaseGrid, IBeginDragHandler, IEndDragHandler, IDragHandler {
  public bool IsDropSuccessful { get; private set; }
  public Action OnDropSuccessful;
  private Canvas canvas;
  private CanvasGroup canvasGroup;
  private Vector2 initialPosition;

  protected override void OnAwake() {
    canvasGroup = GetComponent<CanvasGroup>();

    if (canvas == null)
      canvas = GetComponentInParent<Canvas>();
    if (canvas == null)
      canvas = FindFirstObjectByType<Canvas>();
    if (canvas == null)
      Debug.LogError("No Canvas found for DiceInput.", this);
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

  public void LockDrag() {
    enabled = false;
    canvasGroup.blocksRaycasts = false;
  }

  public void UnlockDrag() {
    enabled = true;
    canvasGroup.blocksRaycasts = true;
  }

  // public void RotateRight() {
  //   int rotatedGridRows = cols;
  //   int rotatedGridCols = rows;

  //   Dice?[,] rotated = new Dice?[rotatedGridRows, rotatedGridCols];
  //   for (int r = 0; r < rows; ++r) {
  //     for (int c = 0; c < cols; ++c) {
  //       rotated[c, rows - r - 1] = Dices[r, c];
  //     }
  //   }

  //   Dices = rotated;
  //   visual.RotateRight();
  // }

  // public void RotateLeft() {
  //   int rows = Dices.GetLength(0);
  //   int cols = Dices.GetLength(1);

  //   Dice?[,] rotated = new Dice?[cols, rows];
  //   for (int r = 0; r < rows; ++r) {
  //     for (int c = 0; c < cols; ++c) {
  //       rotated[cols - 1 - c, r] = Dices[r, c];
  //     }
  //   }

  //   Dices = rotated;
  //   visual.RotateLeft();
  // }
}
