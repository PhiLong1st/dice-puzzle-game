using System;
using UnityEngine;

public class DiceTwoVisualSpawner : ObjectSpawner<DiceTwoVisual> {
  protected override DiceTwoVisual SpawnNew() => Instantiate(_prefab);
}
