using System.Collections.Generic;
using UnityEngine;

public enum DiceInputType { Type1, Type2, Type3 };

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

  public bool IsValidCell(int r, int c) => Grid[r, c] == 1;
}

public class DiceInputManager : MonoBehaviour
{
  public static DiceInputManager Instance { get; private set; }

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
    var gridTemplates = new List<DiceInputTemplate> {
      new DiceInputTemplate(DiceInputType.Type1, new int[,] { { 1 } }),
      new DiceInputTemplate(DiceInputType.Type2, new int[,] { { 1, 1 } }),
      new DiceInputTemplate(DiceInputType.Type3, new int[,] { { 1 }, { 1 } }),
    };

    templateDictionary = new();
    foreach (var diceInputTemplate in gridTemplates)
    {
      templateDictionary.Add(diceInputTemplate.Type, diceInputTemplate);
    }
  }

  private DiceInputTemplate GetRandomTemplate()
  {
    var keys = new List<DiceInputType>(templateDictionary.Keys);
    var randomKey = keys[Random.Range(0, keys.Count)];
    var randomTemplate = templateDictionary[randomKey];
    return randomTemplate;
  }

  public GameObject CreateRandomDiceInput()
  {
    var randomTemplate = GetRandomTemplate();
    if (randomTemplate == null)
    {
      Debug.LogWarning("Randomly selected dice input template was null.");
      return null;
    }

    var rows = randomTemplate.Rows;
    var cols = randomTemplate.Cols;
    var diceTypeGrid = new DiceType?[rows, cols];
    var dicePrefabs = new List<GameObject>();

    for (int r = 0; r < rows; ++r)
    {
      for (int c = 0; c < cols; ++c)
      {
        if (randomTemplate.IsValidCell(r, c))
        {
          var diceType = DiceManager.Instance.RandomDiceType();
          diceTypeGrid[r, c] = diceType;
          dicePrefabs.Add(DiceManager.Instance.GetDicePrefab(diceType));
        }
      }
    }

    var go = new GameObject($"DiceInput_{randomTemplate.Type}");
    var comp = go.AddComponent<DiceInputData>();
    comp.Initialize(randomTemplate.Type, diceTypeGrid);
    var position = new Vector3(0, 0, 0);

    foreach (var prefab in dicePrefabs)
    {
      if (!prefab) continue;
      var tempPrefab = Instantiate(prefab, position, Quaternion.identity);
      position.x += 1f;
      tempPrefab.transform.SetParent(go.transform, false);
    }

    go.AddComponent<DiceInputController>();
    return go;
  }
}