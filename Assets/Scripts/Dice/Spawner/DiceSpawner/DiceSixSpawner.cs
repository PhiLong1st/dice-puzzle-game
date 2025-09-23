using System;
using UnityEngine;

public class DiceSixSpawner : ObjectSpawner<BaseDice> {
  protected override int GetInitialSize() => 5;

}
