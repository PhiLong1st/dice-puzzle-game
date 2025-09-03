using UnityEngine;

public class DiceInput : MonoBehaviour
{
  public DiceInputType Type { get; private set; }
  public DiceType?[,] DiceTypeGrid { get; private set; }
  public int Rows => DiceTypeGrid.GetLength(0);
  public int Cols => DiceTypeGrid.GetLength(1);
  private GameObject[,] dicePrefabsGrid;

  public DiceInput(DiceInputType type, DiceType?[,] template)
  {
    Type = type;
    DiceTypeGrid = template;
  }

  private void Start()
  {
    dicePrefabsGrid = new GameObject[Rows, Cols];

    for (int r = 0; r < Rows; ++r)
    {
      for (int c = 0; c < Cols; ++c)
      {
        var diceType = DiceTypeGrid[r, c];
        if (!diceType.HasValue) continue;
        dicePrefabsGrid[r, c] = DiceManager.Instance.GetDicePrefab(diceType.Value);
      }
    }
  }
}