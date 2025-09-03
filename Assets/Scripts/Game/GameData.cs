using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
  public int Score { get; private set; }
  public int HighScore { get; private set; }
  public Dice[,] DiceGrid { get; private set; }
  public Queue<DiceInputData> inputQueue { get; private set; }
  public Stack<DiceInputData> inputStorage { get; private set; }
}