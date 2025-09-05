using Unity.Burst.Intrinsics;
using UnityEngine;

public class DiceSlotGridData : MonoBehaviour
{
  [SerializeField] public int rows;
  [SerializeField] public int cols;
  public int Rows => rows;
  public int Cols => cols;
  private DiceSlotData[,] GridData;
  public DiceData? GetDiceDataAt(int r, int c)
  {
    return null;
    // Debug.Log($"Get dice data at [{r}, {c}]");
    // return GridData[r, c].DiceData;
  }

  public void SetDiceDataAt(int r, int c, DiceData diceData) => GridData[r, c].SetDiceData(diceData);
}