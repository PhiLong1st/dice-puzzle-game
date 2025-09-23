using UnityEngine;

public abstract class BaseDiceVisual : MonoBehaviour, ISpawnable {
  public abstract DiceVisualType Type { get; }

  public void OnDespawn() {
    gameObject.SetActive(false);
  }

  public void OnSpawn() {
    gameObject.SetActive(true);
  }
}
