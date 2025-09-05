using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
  public int CurrentScore { get; private set; }
  public int HighScore { get; private set; }
  public DiceSlotGridData DiceSlotGrid { get; private set; }
  public DiceData CurrentDiceInput { get; private set; }
  public DiceData NextDiceInput { get; private set; }
  public DiceData KeepingDiceInput { get; private set; }

  public void AddScore(int score)
  {
    if (score <= 0) return;
    CurrentScore += score;
  }

  public void GenerateNextDiceInputData(DiceData data)
  {
    CurrentDiceInput = NextDiceInput;
    NextDiceInput = data;
  }
}
