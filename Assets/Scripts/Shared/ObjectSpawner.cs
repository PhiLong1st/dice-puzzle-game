using UnityEngine;

public abstract class ObjectSpawner<T> : MonoBehaviour where T : Component, ISpawnable {
  [SerializeField] protected T _prefab;
  [SerializeField] protected int _initialSize;
  protected ObjectPool<T> _pool;

  private void Start() {
    Prewarm();
  }

  public T Spawn() {
    if (!_pool.IsEmpty()) {
      return _pool.Get();
    }

    T newObj = SpawnNew();
    return newObj;
  }

  public void Despawn(T obj) {
    _pool.Release(obj);
    obj.OnDespawn();
  }

  protected void Prewarm() {
    _pool = new ObjectPool<T>(transform);
    for (int i = 0; i < _initialSize; ++i) {
      T newObj = SpawnNew();
      _pool.Release(newObj);
    }
  }

  protected abstract T SpawnNew();
}
