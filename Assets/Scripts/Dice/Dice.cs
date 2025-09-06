using UnityEngine;

public class Dice : MonoBehaviour
{
  [SerializeField] private DiceType type;
  public int Value => (int)type;
  public DiceType Type => type;

  public void Initialize(DiceType type)
  {
    this.type = type;
  }
}