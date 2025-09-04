using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceSlot : MonoBehaviour, IDropHandler
{
  public int Row { get; private set; }
  public int Col { get; private set; }

  private RectTransform rectTransform;

  public event Action OnDiceDrop;

  private void Awake()
  {
    rectTransform = GetComponent<RectTransform>();
  }

  public void Initialize(int row, int col)
  {
    Row = row;
    Col = col;
  }

  public void OnDrop(PointerEventData eventData)
  {
    var dragGO = eventData.pointerDrag;
    if (!dragGO) return;

    var draggedRect = dragGO.GetComponent<RectTransform>();
    draggedRect.SetParent(rectTransform, worldPositionStays: false);
    draggedRect.anchoredPosition = Vector2.zero;
    draggedRect.localRotation = Quaternion.identity;
    draggedRect.localScale = Vector3.one;

    var dice = dragGO.GetComponent<Dice>();
    if (dice) Debug.Log($"Drop a {dice.Value} dice at cell [{Row}, {Col}]");

    OnDiceDrop?.Invoke();
  }

}