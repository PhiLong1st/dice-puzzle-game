using UnityEngine;

public abstract class ObjectSpawner<T> : MonoBehaviour where T : Component, ISpawnable {
  [SerializeField] protected T _prefab;
  protected ObjectPool<T> _pool;

  private void Start() {
    Prewarm();
  }

  public virtual T Spawn() {
    if (!_pool.IsEmpty()) {
      var obj = _pool.Get();
      obj.OnSpawn();
      return obj;
    }

    T newObj = SpawnNew();
    newObj.OnSpawn();
    return newObj;
  }

  public virtual void Despawn(T obj) {
    _pool.Release(obj);
    obj.OnDespawn();
  }

  private void Prewarm() {
    _pool = new ObjectPool<T>(transform);
    int initialSize = GetInitialSize();
    for (int i = 0; i < initialSize; ++i) {
      T newObj = SpawnNew();
      Despawn(newObj);
    }
  }

  private T SpawnNew() {
    T newObj = Instantiate(_prefab);
    return newObj;
  }

  protected abstract int GetInitialSize();
}
