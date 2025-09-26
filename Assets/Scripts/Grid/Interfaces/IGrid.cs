using UnityEngine;

public interface IGrid<T> {
  int Rows { get; }
  int Cols { get; }
  T[,] Grid { get; }
  T Get(int row, int col);
  void Set(int row, int col, T item);
  void Clear(int row, int col);
  bool IsInBounds(int row, int col);
  bool IsEmpty(int row, int col);
}
