using System;
using UnityEngine;

public class DiceThreeSpawner : ObjectSpawner<DiceThree> {
  protected override DiceThree SpawnNew() => Instantiate(_prefab);
}
