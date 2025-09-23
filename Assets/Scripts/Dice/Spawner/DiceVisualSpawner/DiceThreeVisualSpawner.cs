using System;
using UnityEngine;

public class DiceThreeVisualSpawner : ObjectSpawner<BaseDiceVisual> {
  protected override int GetInitialSize() => 5;
}
