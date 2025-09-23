using System;
using UnityEngine;

public class DiceOneSpawner : ObjectSpawner<DiceOne> {
  protected override DiceOne SpawnNew() => Instantiate(_prefab);
}
