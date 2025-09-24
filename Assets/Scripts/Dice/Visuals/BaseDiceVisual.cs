using UnityEngine;

public abstract class BaseDiceVisual : MonoBehaviour, ISpawnable<BaseDiceVisual> {
  public abstract DiceVisualType Type { get; }

  public void OnDespawn() {
    gameObject.SetActive(false);
  }

  public void OnSpawn() {
    gameObject.SetActive(true);
  }

  public BaseDiceVisual CreateFn() {
    return Instantiate(this);
  }

  public void ResetFn() {
    Debug.Log("Reset visual");
  }
}
