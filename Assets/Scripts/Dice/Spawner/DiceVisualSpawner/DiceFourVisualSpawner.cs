using System;
using UnityEngine;

public class DiceFourVisualSpawner : ObjectSpawner<DiceFourVisual> {
  protected override DiceFourVisual SpawnNew() => Instantiate(_prefab);
}
