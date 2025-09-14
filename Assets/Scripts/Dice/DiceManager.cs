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
}
