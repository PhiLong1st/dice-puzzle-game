using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct DiceVisualSpawnerPrefab {
  public DiceVisualType Type;
  public GameObject Prefab;
}

public class DiceVisualSpawner : MonoBehaviour {
  public static DiceVisualSpawner Instance { get; private set; }
  [SerializeField] private List<DiceVisualSpawnerPrefab> _spawnerPrefabs;
  private Dictionary<DiceVisualType, ObjectSpawner<BaseDiceVisual>> _spawners;

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
      DiceVisualType key = prefab.Type;
      var value = prefab.Prefab.GetComponent<ObjectSpawner<BaseDiceVisual>>();

      if (_spawners.ContainsKey(key)) {
        continue;
      }

      _spawners.Add(key, value);
    }
  }

  public BaseDiceVisual SpawnByType(DiceVisualType type) => _spawners[type].Spawn();

  public void Despawn(BaseDiceVisual visual) {
    DiceVisualType diceType = visual.Type;
    _spawners[diceType].Despawn(visual);
  }
}
