using System;
using UnityEngine;

public class DiceSixVisualSpawner : ObjectSpawner<BaseDiceVisual> {
  protected override int GetInitialSize() => 5;
}
