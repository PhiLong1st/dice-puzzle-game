using System.Collections.Generic;
using UnityEngine;

public sealed class SimplePlacementHandler : IPlacementHandler<BaseDice> {
  public bool TryPlace(IGrid<BaseDice> board, BaseDice[,] shape, (int row, int col) anchorCell, out ICollection<(int row, int col)> dropCells) {
    dropCells = new List<(int row, int col)>();

    int rows = shape.GetLength(0);
    int cols = shape.GetLength(1);
    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        if (!shape[r, c]) {
          continue;
        }

        int dropRow = r + anchorCell.row;
        int dropCol = c + anchorCell.col;
        if (!board.IsInBounds(dropRow, dropCol)) {
          return false;
        }

        if (!board.IsEmpty(dropRow, dropCol)) {
          return false;
        }
      }
    }

    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        if (!shape[r, c])
          continue;

        int dropRow = r + anchorCell.row;
        int dropCol = c + anchorCell.col;
        board.Set(dropRow, dropCol, shape[r, c]);
        dropCells.Add((dropRow, dropCol));
      }
    }

    return true;
  }
}
