using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct DiceSpawnerPrefab {
  public DiceType Type;
  public GameObject Prefab;
}

public class DiceSpawner : MonoBehaviour {
  public static DiceSpawner Instance { get; private set; }
  [SerializeField] private List<DiceSpawnerPrefab> _spawnerPrefabs;
  private Dictionary<DiceType, ObjectSpawner<BaseDice>> _spawners;

  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
  }

  private void Start() {
    InitSpawners();
  }

  private void InitSpawners() {
    _spawners = new();
    foreach (var prefab in _spawnerPrefabs) {
      DiceType key = prefab.Type;
      var value = prefab.Prefab.GetComponent<ObjectSpawner<BaseDice>>();

      if (_spawners.ContainsKey(key)) {
        continue;
      }

      _spawners.Add(key, value);
    }
  }

  public BaseDice SpawnByType(DiceType type) => _spawners[type].Spawn();

  public void Despawn(BaseDice dice) {
    DiceType diceType = dice.Type;
    _spawners[diceType].Despawn(dice);
  }
}
