using System;
using UnityEngine;

public class DiceFourVisualSpawner : ObjectSpawner<BaseDiceVisual> {
  protected override int GetInitialSize() => 5;
}
