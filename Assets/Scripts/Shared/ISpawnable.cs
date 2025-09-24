public interface ISpawnable<T> {
  public void OnSpawn();
  public void ResetFn();
  public T CreateFn();
}
