public interface ISpawnable<T> {
  public void OnSpawn();
  public void OnDespawn();
  public T CreateFn();
  public void ResetFn();
}
