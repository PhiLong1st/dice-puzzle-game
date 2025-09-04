using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceSlot : MonoBehaviour, IDropHandler
{
  public int Row { get; private set; }
  public int Col { get; private set; }

  public event Action OnDiceDrop;

  public void Initialize(int row, int col)
  {
    Row = row;
    Col = col;
  }

  public void OnDrop(PointerEventData eventData)
  {
    if (eventData.pointerDrag != null)
    {
      eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
      var dice = eventData.pointerDrag.GetComponent<Dice>();
      Debug.Log($"Drop a {dice.Value} dice at cell [{Row}, {Col}]");
      OnDiceDrop?.Invoke();
    }
  }
}