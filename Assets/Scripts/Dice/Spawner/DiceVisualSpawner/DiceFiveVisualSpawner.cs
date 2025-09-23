using System;
using UnityEngine;

public class DiceFiveVisualSpawner : ObjectSpawner<DiceFiveVisual> {
  protected override DiceFiveVisual SpawnNew() => Instantiate(_prefab);
}
