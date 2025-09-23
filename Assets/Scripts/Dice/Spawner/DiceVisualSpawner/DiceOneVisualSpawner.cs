using System;
using UnityEngine;

public class DiceOneVisualSpawner : ObjectSpawner<BaseDiceVisual> {
  protected override int GetInitialSize() => 5;
}
