using System;
using UnityEngine;

public class DiceFiveVisualSpawner : ObjectSpawner<BaseDiceVisual> {
  protected override int GetInitialSize() => 5;

}
