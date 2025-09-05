using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
  public int CurrentScore { get; private set; }
  public int HighScore { get; private set; }
  public DiceSlotGrid DiceSlotGrid { get; private set; }
  public DiceInputQueue DiceInputQueue { get; private set; }

  public void AddScore(int score)
  {
    if (score <= 0) return;
    CurrentScore += score;
  }
}
