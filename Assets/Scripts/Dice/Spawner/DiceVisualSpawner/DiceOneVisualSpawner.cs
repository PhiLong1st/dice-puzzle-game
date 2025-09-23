using System;
using UnityEngine;

public class DiceOneVisualSpawner : ObjectSpawner<DiceOneVisual> {
  protected override DiceOneVisual SpawnNew() => Instantiate(_prefab);
}
