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
        diceSlotGrid[r, c] = Instantiate(diceSlotPrefab).GetComponent<DiceSlot>();
        diceSlotGrid[r, c].SetCoordinates(r, c);
        if (r == 2 && c == 2)
        {
          diceSlotGrid[r, c].PlaceDice(DiceManager.Instance.GetDicePrefab(DiceType.Six).GetComponent<Dice>());
        }
        else
        {
          diceSlotGrid[r, c].PlaceDice(DiceManager.Instance.DefaultDice);
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

    var diceInput = diceGO.GetComponent<DiceInput>();
    var draggedRect = diceGO.GetComponent<RectTransform>();

    Vector2 dropPos = default;
    var isDropOk = IsFullyInsideBounds(rectTransform, draggedRect)
                && !IsDiceCollapse(rectTransform, draggedRect, out dropPos);

    if (!isDropOk) return;
    diceInput.NotifyDropOk(isDropOk);
    draggedRect.anchoredPosition = dropPos;
  }

  private bool IsFullyInsideBounds(RectTransform container, RectTransform target)
  {
    Vector2 containerTopLeftCorner = container.TopLeftCorner();
    Vector2 containerBottomRightCorner = container.BottomRightCorner();

    Vector2 topLeftCorner = target.TopLeftCorner();
    Vector2 topRightCorner = target.TopRightCorner();
    Vector2 bottomLeftCorner = target.BottomLeftCorner();
    Vector2 bottomRightCorner = target.BottomRightCorner();

    bool Inside(Vector2 p) =>
        containerTopLeftCorner.x <= p.x && p.x <= containerBottomRightCorner.x &&
        containerBottomRightCorner.y <= p.y && p.y <= containerTopLeftCorner.y;

    return Inside(topLeftCorner) && Inside(topRightCorner) && Inside(bottomLeftCorner) && Inside(bottomRightCorner);
  }

  private bool IsDiceCollapse(RectTransform container, RectTransform target, out Vector2 dropPosition)
  {
    (int cellRow, int cellCol) cell = (-1, -1);
    float minimumDistance = float.MaxValue;
    Vector2 containerTopLeftCorner = container.TopLeftCorner();
    Vector2 targerTopLeftCorner = target.TopLeftCorner();
    dropPosition = new Vector2();

    for (int r = 0; r < rows; ++r)
    {
      for (int c = 0; c < cols; ++c)
      {
        RectTransform slotRectTransform = diceSlotGrid[r, c].GetComponent<RectTransform>();
        Vector2 slotTopLeftCorner = containerTopLeftCorner + slotRectTransform.TopLeftCorner();
        float distance = (targerTopLeftCorner - slotTopLeftCorner).magnitude;
        if (minimumDistance > distance)
        {
          minimumDistance = distance;
          dropPosition = slotTopLeftCorner;
          cell = (r, c);
        }
      }
    }

    Dice[,] diceInputs = target.gameObject.GetComponent<DiceInput>().DiceContainer;

    bool isCollapse = false;
    int diceInputsRows = diceInputs.GetLength(0);
    int diceInputsCols = diceInputs.GetLength(1);
    for (int r = cell.cellRow; r < cell.cellRow + diceInputsRows; ++r)
    {
      for (int c = cell.cellCol; c < cell.cellCol + diceInputsCols; ++c)
      {
        isCollapse |= diceInputs[r - cell.cellRow, c - cell.cellCol].Type != DiceType.Zero && diceSlotGrid[r, c].Dice.Type != DiceType.Zero;
      }
    }

    dropPosition.x += target.pivot.x * target.sizeDelta.x;
    dropPosition.y -= target.pivot.y * target.sizeDelta.y;
    return isCollapse;
  }
}