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

  public void ResetFn() {
    if (_visual != null) {
      DiceVisualSpawner.Instance.Release(_visual);
    }
    gameObject.SetActive(false);
  }

  protected abstract DiceVisualType GetCurrentVisualType();
  public abstract DiceType? GetNextDiceType();

  public BaseDice CreateFn() => Instantiate(this);

  public void Release() => DiceSpawner.Instance.Release(this);
}
