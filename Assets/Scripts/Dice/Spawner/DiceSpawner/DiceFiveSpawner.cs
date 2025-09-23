using System;
using UnityEngine;

public class DiceFiveSpawner : ObjectSpawner<DiceFive> {
  protected override DiceFive SpawnNew() => Instantiate(_prefab);
}
