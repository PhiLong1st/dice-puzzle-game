using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour {
  public static DiceSpawner Instance { get; private set; }
  [SerializeField] private List<BaseDice> _dicePrefabs;
  private Dictionary<DiceType, ObjectPool<BaseDice>> _spawners;

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
    foreach (BaseDice prefab in _dicePrefabs) {
      DiceType key = prefab.Type;
      var poolGO = new GameObject($"{key} Pool");
      poolGO.transform.SetParent(transform, false);
      var pool = new ObjectPool<BaseDice>(prefab.CreateFn, poolGO.transform);

      if (_spawners.ContainsKey(key)) {
        continue;
      }

      _spawners.Add(key, pool);
    }
  }

  public BaseDice SpawnByType(DiceType type) => _spawners[type].Get();

  public BaseDice SpawnRandom() {
    DiceType diceType = EnumUtils.RandomEnumValue<DiceType>();
    return _spawners[diceType].Get();
  }

  public void Despawn(BaseDice dice) {
    DiceType diceType = dice.Type;
    _spawners[diceType].Release(dice);
  }
}
