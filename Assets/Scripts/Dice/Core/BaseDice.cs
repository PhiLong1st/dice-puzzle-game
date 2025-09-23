using UnityEngine;

public abstract class BaseDice : MonoBehaviour, ISpawnable {
  public abstract DiceType Type { get; }
  public int Point => (int)Type;

  private BaseDiceVisual visual;
  private RectTransform rectTransform;

  private void Awake() {
    rectTransform = GetComponent<RectTransform>();
  }

  private void InitVisual() {
    visual = DiceVisualSpawner.Instance.SpawnByType(GetCurrentVisualType());
    visual.GetComponent<RectTransform>().SetParent(rectTransform, false);
  }

  public void OnSpawn() {
    InitVisual();
    gameObject.SetActive(true);
  }

  public void OnDespawn() {
    if (visual != null) {
      DiceVisualSpawner.Instance.Despawn(visual);
    }
    gameObject.SetActive(false);
  }

  protected abstract DiceVisualType GetCurrentVisualType();
}
