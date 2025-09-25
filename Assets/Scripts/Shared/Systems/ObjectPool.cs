using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component, ISpawnable<T> {
  private Stack<T> _pool;
  private Transform _parent;
  private int _initialSize;

  private Func<T> _objectCreateFn;

  public ObjectPool(Func<T> createFn, Transform parent, int initialSize = 5) {
    _objectCreateFn = createFn;
    _initialSize = initialSize;
    _parent = parent;
    Init();
  }

  public T Get() {
    if (_pool.Count == 0) {
      T newObj = _objectCreateFn();
      newObj.OnSpawn();
      return newObj;
    }

    var obj = _pool.Pop();
    obj.OnSpawn();
    return obj;
  }

  public void Release(T obj) {
    obj.ResetFn();
    _pool.Push(obj);
    obj.transform.SetParent(_parent, false);
  }

  private void Init() {
    _pool = new();
    for (int i = 0; i < _initialSize; ++i) {
      T newObj = _objectCreateFn();
      Release(newObj);
    }
  }
}
