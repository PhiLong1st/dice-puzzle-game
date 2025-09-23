using System;
using UnityEngine;

public class DiceThreeSpawner : ObjectSpawner<BaseDice> {
  protected override int GetInitialSize() => 5;
}
