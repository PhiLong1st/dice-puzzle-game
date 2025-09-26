using System.Collections.Generic;

public interface IMergeResolver<T> {
  public void Resolve(IGrid<T> board, ICollection<(int row, int col)> seeds);
}
