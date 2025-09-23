using System;
using UnityEngine;

public class DiceTwoVisualSpawner : ObjectSpawner<BaseDiceVisual> {
  protected override int GetInitialSize() => 5;
}
