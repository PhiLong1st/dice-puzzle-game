using System;
using UnityEngine;

public class DiceSixVisualSpawner : ObjectSpawner<DiceSixVisual> {
  protected override DiceSixVisual SpawnNew() => Instantiate(_prefab);
}
