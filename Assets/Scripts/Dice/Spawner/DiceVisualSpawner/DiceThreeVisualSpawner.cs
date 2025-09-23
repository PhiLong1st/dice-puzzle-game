using System;
using UnityEngine;

public class DiceThreeVisualSpawner : ObjectSpawner<DiceThreeVisual> {
  protected override DiceThreeVisual SpawnNew() => Instantiate(_prefab);
}
