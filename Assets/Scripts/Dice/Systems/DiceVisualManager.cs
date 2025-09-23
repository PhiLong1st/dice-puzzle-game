using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct DiceVisualPrefab {
  public DiceType type;
  public GameObject go;
}

public class DiceVisualManager : MonoBehaviour {
  public static DiceVisualManager Instance { get; private set; }

  [SerializeField] private List<DiceVisualPrefab> prefabs;
  private Dictionary<DiceType, BaseDiceVisual> cachedVisuals;

  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);

    BuildCachedVisuals();
  }

  public BaseDiceVisual GetDiceVisualByType(DiceType type) => Instantiate(cachedVisuals[type]);

  private void BuildCachedVisuals() {
    cachedVisuals = new();
    foreach (DiceVisualPrefab prefab in prefabs) {
      var visual = prefab.go.GetComponent<BaseDiceVisual>();
      cachedVisuals.Add(prefab.type, visual);
      // Debug.Log($"{visual.Type}");
    }
  }
}
