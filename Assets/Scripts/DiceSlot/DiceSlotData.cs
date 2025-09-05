using UnityEngine;

public class DiceSlotData : MonoBehaviour
{
  public int AtRow { get; private set; }
  public int AtCol { get; private set; }
  public DiceData? DiceData { get; private set; }

  public void Initialize(int row, int col, DiceData? diceData = null)
  {
    AtRow = row;
    AtCol = col;
    DiceData = diceData;
    Debug.Log($"Initialize dice slot data in [{row}, {col}] with {diceData}.");
  }

  public void SetDiceData(DiceData diceData)
  {
    DiceData = diceData;
    Debug.Log($"Drop a {DiceData.Value} dice at cell [{AtRow}, {AtCol}].");
  }
}