public interface ISpawnable<T> {
  public void OnSpawn();
  public void ResetFn();
  public void Release();
  public T CreateFn();
}
