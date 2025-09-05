using UnityEngine;

public class DiceData : MonoBehaviour
{
  [SerializeField] private int value;
  [SerializeField] private DiceType type;
  public int Value => value;
  public DiceType Type => type;
}