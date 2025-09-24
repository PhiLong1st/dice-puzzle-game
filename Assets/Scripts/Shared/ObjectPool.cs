using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : ISpawnable<T> {
  private Transform _parent;
  private Stack<T> _pool;
  private int _initialSize;

  Func<T> createFn;
  Action resetFn;

  public ObjectPool(Func<T> createFn, Action resetFn, Transform _parent) {

  }

  public T Get() {
    if (_pool.Count == 0) {
      var obj = _pool.Pop();
      obj.OnSpawn();
      return obj;
    }

    T newObj = createFn();
    newObj.OnSpawn();
    return newObj;
  }

  public void Release(T obj) {
    obj.OnDespawn();
    _pool.Push(obj);
  }

  private void Init() {
    _pool = new();
    for (int i = 0; i < _initialSize; ++i) {
      T newObj = createFn();
      Release(newObj);
    }
  }
}
