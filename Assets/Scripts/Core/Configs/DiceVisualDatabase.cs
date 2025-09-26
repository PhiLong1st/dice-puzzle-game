using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dice/Visual Database")]
public class DiceVisualDatabase : ScriptableObject {
  [Serializable]
  public struct Entry {
    public DiceType type;
    public GameObject visual;
  }

  [SerializeField] private List<Entry> entries = new();
  private Dictionary<DiceType, Entry> _map;

  void OnEnable() {
    _map = new();
    foreach (var e in entries) {
      _map[e.type] = e;
    }
  }

  public GameObject Get(DiceType type) => _map.TryGetValue(type, out var e) ? e.visual : null;
}
