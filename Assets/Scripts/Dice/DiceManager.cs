using System;
using System.Collections.Generic;
using UnityEngine;

public enum DiceType { One, Two, Three, Four, Five, Six };

[Serializable]
public struct DicePrefab {
  public DiceType Type;
  public GameObject Prefab;
}

public class DiceManager : MonoBehaviour {
  public static DiceManager Instance { get; private set; }

  [SerializeField] private DicePrefab[] dicePrefabs;
  [SerializeField] private GameObject emptyDicePrefab;
  public GameObject EmptyDicePrefab => Instantiate(emptyDicePrefab);
  private Dictionary<DiceType, GameObject> cachedDices;

  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
    BuildCachedDices();
  }

  private void BuildCachedDices() {
    cachedDices = new();
    foreach (var dicePrefab in dicePrefabs) {
      if (cachedDices.ContainsKey(dicePrefab.Type))
        continue;

      if (dicePrefab.Prefab == null) {
        Debug.LogError($"DICE MANAGER: Missing Prefab for ({dicePrefab.Type}).", this);
        continue;
      }
      cachedDices.Add(dicePrefab.Type, dicePrefab.Prefab);
    }
  }

  public GameObject GetDicePrefab(DiceType diceType) {
    cachedDices.TryGetValue(diceType, out GameObject dicePrefab);

    if (dicePrefab == null) {
      Debug.LogError($"DICE MANAGER: No prefab registered for {diceType}.", this);
    }

    var clonedPrefab = Instantiate(dicePrefab);
    return clonedPrefab;
  }

  public DiceType? GetNextDiceType(DiceType diceType) {
    switch (diceType) {
      case DiceType.One:
        return DiceType.Two;
      case DiceType.Two:
        return DiceType.Three;
      case DiceType.Three:
        return DiceType.Four;
      case DiceType.Four:
        return DiceType.Five;
      case DiceType.Five:
        return DiceType.Six;
      case DiceType.Six:
        return null;
      default:
        return null;
    }
  }

  public Dice? GetNextDice(DiceType diceType) {
    Debug.Log("here");
    DiceType? nextDiceType = GetNextDiceType(diceType);
    if (nextDiceType == null)
      return null;

    cachedDices.TryGetValue(nextDiceType.Value, out GameObject dicePrefab);

    if (dicePrefab == null) {
      Debug.LogError($"DICE MANAGER: No prefab registered for {diceType}.", this);
    }

    return Instantiate(dicePrefab).GetComponent<Dice>();
  }
}
