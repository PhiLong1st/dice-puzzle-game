using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component, ISpawnable {
  private readonly Stack<T> _pool;
  private readonly Transform _parent;

  public ObjectPool(Transform parent) {
    _pool = new();
    _parent = parent;
  }

  public bool IsEmpty() => _pool.Count == 0;

  public T Get() => _pool.Pop();

  public void Release(T obj) {
    _pool.Push(obj);
    obj.OnDespawn();
    obj.transform.SetParent(_parent, false);
  }
}
