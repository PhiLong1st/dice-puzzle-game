using System.Collections.Generic;
using UnityEngine;

public enum DiceInputType { Type1, Type2, Type3, Type4, Type5 };

public class DiceInputTemplate
{
  public DiceInputType Type { get; private set; }
  public int[,] Grid { get; private set; }
  public int Rows => Grid.GetLength(0);
  public int Cols => Grid.GetLength(1);

  public DiceInputTemplate(DiceInputType type, int[,] grid)
  {
    Type = type;
    Grid = grid;
  }
}

public class DiceInputManager : MonoBehaviour
{
  public DiceInputManager Instance { get; private set; }

  private Dictionary<DiceInputType, DiceInputTemplate> templateDictionary;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
  }


  private void Start()
  {
    templateDictionary = new();

    var templates = new List<DiceInputTemplate> {
      new DiceInputTemplate(DiceInputType.Type1, new int[,] { { 1 } }),
      new DiceInputTemplate(DiceInputType.Type2, new int[,] { { 1, 1 } }),
      new DiceInputTemplate(DiceInputType.Type3, new int[,] { { 1 }, { 1 } }),
      new DiceInputTemplate(DiceInputType.Type4, new int[,] { { 1, 0 }, { 0, 1 } }),
      new DiceInputTemplate(DiceInputType.Type5, new int[,] { { 0, 1 }, { 1, 0 } }),
    };

    foreach (var diceInputTemplate in templates)
    {
      templateDictionary.Add(diceInputTemplate.Type, diceInputTemplate);
    }
  }

  public DiceInput GenerateRandomDiceInput()
  {
    var keys = new List<DiceInputType>(templateDictionary.Keys);
    var randomKey = keys[Random.Range(0, keys.Count)];
    var template = templateDictionary[randomKey];

    if (template == null)
    {
      Debug.LogWarning("Randomly selected dice input template was null.");
      return null;
    }

    var rows = template.Rows;
    var cols = template.Cols;
    var diceTypeGrid = new DiceType?[rows, cols];

    for (int r = 0; r < rows; ++r)
    {
      for (int c = 0; c < cols; ++c)
      {
        if (template.Grid[r, c] == 1)
        {
          diceTypeGrid[r, c] = DiceManager.Instance.RandomDiceType();
        }
      }
    }

    return new DiceInput(template.Type, diceTypeGrid);
  }
}