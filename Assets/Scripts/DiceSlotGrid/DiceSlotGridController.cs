using UnityEngine;

public class DiceSlotGridController : MonoBehaviour
{
  [SerializeField] private GameObject diceSlotPrefab;
  private RectTransform rectTransform;
  private DiceSlotGridData gridData;

  private void Start()
  {
    gridData = GetComponent<DiceSlotGridData>();
    rectTransform = GetComponent<RectTransform>();
    BuildGrid();
  }

  private void BuildGrid()
  {
    if (!diceSlotPrefab) return;

    var rows = gridData.Rows;
    var cols = gridData.Cols;

    for (int r = 0; r < rows; ++r)
    {
      for (int c = 0; c < cols; ++c)
      {
        var clonedGO = Instantiate(diceSlotPrefab);
        var clonedGORect = clonedGO.GetComponent<RectTransform>();
        clonedGORect.SetParent(rectTransform, worldPositionStays: false);

        var diceSlot = clonedGO.GetComponent<DiceSlotData>();
        var diceData = gridData.GetDiceDataAt(r, c);
        diceSlot.Initialize(r, c, diceData);
      }
    }
  }
}