using UnityEngine;

public abstract class BaseDiceVisual : MonoBehaviour, ISpawnable<BaseDiceVisual> {
  public abstract DiceVisualType Type { get; }

  public void ResetFn() {
    Debug.Log("Reset visual");
    gameObject.SetActive(false);
  }

  public void OnSpawn() {
    gameObject.SetActive(true);
  }

  public BaseDiceVisual CreateFn() {
    return Instantiate(this);
  }
}
