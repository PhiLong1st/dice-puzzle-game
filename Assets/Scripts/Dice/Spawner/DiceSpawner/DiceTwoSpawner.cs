using System;
using UnityEngine;

public class DiceTwoSpawner : ObjectSpawner<DiceTwo> {
  protected override DiceTwo SpawnNew() => Instantiate(_prefab);
}
