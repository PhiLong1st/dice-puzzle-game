using System;
using UnityEngine;

public class DiceSixSpawner : ObjectSpawner<DiceSix> {
  protected override DiceSix SpawnNew() => Instantiate(_prefab);
}
