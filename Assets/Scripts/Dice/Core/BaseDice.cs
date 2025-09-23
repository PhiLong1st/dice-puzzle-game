using UnityEngine;

public abstract class BaseDice : MonoBehaviour, ISpawnable {
  public abstract DiceType Type { get; }
  public int Point => (int)Type;

  private BaseDiceVisual visualGO;
  private RectTransform rectTransform;

  private void Awake() {
    rectTransform = GetComponent<RectTransform>();
  }

  private void Start() {
    InitVisual();
  }

  private void InitVisual() {
    visualGO = DiceVisualManager.Instance.GetDiceVisualByType(Type);
    visualGO.GetComponent<RectTransform>().SetParent(rectTransform, false);
  }

  public void OnSpawn() {
    gameObject.SetActive(false);
  }

  public void OnDespawn() {
    gameObject.SetActive(false);
  }
}
