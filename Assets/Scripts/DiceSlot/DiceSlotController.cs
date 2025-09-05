using UnityEngine;
using UnityEngine.EventSystems;

public class DiceSlotController : MonoBehaviour, IDropHandler
{
  private RectTransform rectTransform;
  private DiceSlotData diceSlotData;

  private void Awake()
  {
    rectTransform = GetComponent<RectTransform>();
    diceSlotData = GetComponent<DiceSlotData>();
  }

  public void OnDrop(PointerEventData eventData)
  {
    var diceGO = eventData.pointerDrag;

    if (!diceGO) return;

    var diceGOdata = diceGO.GetComponent<DiceData>();
    diceSlotData.SetDiceData(diceGOdata);

    var draggedRect = diceGO.GetComponent<RectTransform>();
    draggedRect.SetParent(rectTransform, worldPositionStays: false);
    draggedRect.anchoredPosition = Vector2.zero;
    draggedRect.localRotation = Quaternion.identity;
    draggedRect.localScale = Vector3.one;
  }
}