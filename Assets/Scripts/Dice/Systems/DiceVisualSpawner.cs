using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceVisualSpawner : MonoBehaviour {
  public static DiceVisualSpawner Instance { get; private set; }
  [SerializeField] private List<BaseDiceVisual> _diceVisualPrefabs;
  private Dictionary<DiceVisualType, ObjectPool<BaseDiceVisual>> _spawners;

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
    foreach (BaseDiceVisual prefab in _diceVisualPrefabs) {
      DiceVisualType key = prefab.Type;
      var poolGO = new GameObject($"{key} Visual Pool");
      poolGO.transform.SetParent(transform, false);
      var pool = new ObjectPool<BaseDiceVisual>(prefab.CreateFn, prefab.ResetFn, poolGO.transform);

      if (_spawners.ContainsKey(key)) {
        continue;
      }

      _spawners.Add(key, pool);
    }
  }

  public BaseDiceVisual SpawnByType(DiceVisualType type) {
    return _spawners[type].Get();
  }

  public void Despawn(BaseDiceVisual visual) {
    DiceVisualType diceType = visual.Type;
    _spawners[diceType].Release(visual);
  }
}
