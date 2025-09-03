using System;
using System.Collections.Generic;
using UnityEngine;

public enum DiceType { One = 1, Two, Three, Four, Five, Six };

[Serializable]
public struct DicePrefab
{
  public DiceType Type;
  public GameObject Prefab;
}

public class DiceManager : MonoBehaviour
{
  public static DiceManager Instance { get; private set; }
  [SerializeField] private DicePrefab[] dicePrefabs;
  private Dictionary<DiceType, GameObject> diceDictionary;

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
    diceDictionary = new();
    foreach (var dicePrefab in dicePrefabs)
    {
      if (diceDictionary.ContainsKey(dicePrefab.Type)) continue;

      if (dicePrefab.Prefab == null)
      {
        Debug.LogWarning($"DiceManager: Missing Prefab for ({dicePrefab.Type}).", this);
        continue;
      }

      diceDictionary.Add(dicePrefab.Type, dicePrefab.Prefab);
    }
  }

  public GameObject GetDicePrefab(DiceType diceType)
  {
    diceDictionary.TryGetValue(diceType, out GameObject dicePrefab);

    if (dicePrefab == null)
    {
      Debug.LogError($"DiceManager: No prefab registered for {diceType}.", this);
    }

    return dicePrefab;
  }

  public DiceType RandomDiceType() => (DiceType)UnityEngine.Random.Range(1, 7);
}