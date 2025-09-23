using System;
using UnityEngine;

public class DiceTwoSpawner : ObjectSpawner<BaseDice> {
  protected override int GetInitialSize() => 5;

}
