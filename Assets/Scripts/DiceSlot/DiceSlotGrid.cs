using UnityEngine;

public class DiceSlotGrid : MonoBehaviour
{
  [SerializeField] public int rows;
  [SerializeField] public int cols;
  public int Rows => rows;
  public int Cols => cols;
  private DiceSlot[,] GridData;

  public bool TryGetDataAt(int r, int c, out DiceSlot? data)
  {
    data = null;
    if (!InBounds(r, c) || GridData[r, c] == null) return false;
    data = GridData[r, c];
    return true;
  }

  private bool InBounds(int r, int c) => 0 <= r && r < rows && 0 <= c && c <= cols;

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

    for (int r = 0; r < rows; ++r)
    {
      for (int c = 0; c < cols; ++c)
      {
        var clonedGO = Instantiate(diceSlotPrefab);
        var clonedGORect = clonedGO.GetComponent<RectTransform>();
        clonedGORect.SetParent(rectTransform, worldPositionStays: false);

        var diceSlot = clonedGO.GetComponent<DiceSlot>();
        diceSlot.Initialize(r, c);
      }
    }
  }
}