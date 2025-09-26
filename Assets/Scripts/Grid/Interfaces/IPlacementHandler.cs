using System.Collections.Generic;

public interface IPlacementHandler<T> {
  bool TryPlace(IGrid<T> board, BaseDice[,] shape, (int row, int col) anchorCell, out ICollection<(int row, int col)> dropCells);
}
