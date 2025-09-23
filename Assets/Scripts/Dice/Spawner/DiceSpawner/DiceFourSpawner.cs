using System;
using UnityEngine;

public class DiceFourSpawner : ObjectSpawner<DiceFour> {
  protected override DiceFour SpawnNew() => Instantiate(_prefab);
}
