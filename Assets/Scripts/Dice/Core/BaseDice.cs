using UnityEngine;

public abstract class BaseDice : MonoBehaviour, ISpawnable<BaseDice> {
  public abstract DiceType Type { get; }
  public int Point => (int)Type;

  private BaseDiceVisual _visual;
  private RectTransform _rectTransform;

  private void Awake() {
    _rectTransform = GetComponent<RectTransform>();
  }

  private void InitVisual() {
    _visual = DiceVisualSpawner.Instance.SpawnByType(GetCurrentVisualType());
    _visual.GetComponent<RectTransform>().SetParent(_rectTransform, false);
  }

  public void OnSpawn() {
    InitVisual();
    gameObject.SetActive(true);
  }

  public void OnDespawn() {
    if (_visual != null) {
      DiceVisualSpawner.Instance.Despawn(_visual);
    }
    gameObject.SetActive(false);
  }

  protected abstract DiceVisualType GetCurrentVisualType();

  public BaseDice CreateFn() => Instantiate(this);

  public void ResetFn() {
    Debug.Log("Reset dice");
  }
}
