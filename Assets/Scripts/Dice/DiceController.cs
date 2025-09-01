using UnityEngine;

public class DiceController : MonoBehaviour
{
  private Dice dice;
  private DiceVisuals diceVisuals;

  private void Start()
  {
    dice = GetComponent<Dice>();
    diceVisuals = GetComponent<DiceVisuals>();
  }
}