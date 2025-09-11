using UnityEngine;

public class GameData : MonoBehaviour
{
  public int CurrentScore { get; private set; }
  public int HighScore { get; private set; }
  public DiceSlotGrid DiceSlotGrid { get; private set; }
  public IncomingDice CurrentDiceInput;
  public IncomingDice NextDiceInput;

  public Vector2 CurrentDiceInputGOPosition { get; private set; } = new Vector2(700f, 0f);
  public Vector2 NextDiceInputGOPosition { get; private set; } = new Vector2(700f, -205f);

  public void AddScore(int score)
  {
    if (score <= 0) return;
    CurrentScore += score;
  }
}
