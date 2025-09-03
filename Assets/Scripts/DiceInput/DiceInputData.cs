using UnityEngine;

public class DiceInputData : MonoBehaviour
{
  public DiceInputType Type { get; private set; }
  public DiceType?[,] DiceTypeGrid { get; private set; }
  public int Rows => DiceTypeGrid.GetLength(0);
  public int Cols => DiceTypeGrid.GetLength(1);

  public void Initialize(DiceInputType type, DiceType?[,] diceTypeGrid)
  {
    Type = type;
    DiceTypeGrid = (DiceType?[,])diceTypeGrid.Clone();
  }
}