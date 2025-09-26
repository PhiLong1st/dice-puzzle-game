using System.Collections.Generic;

public class MatchAndUpgradeMergeResolver : IMergeResolver<BaseDice> {

  private const int _numberCellMatching = 3;

  public void Resolve(IGrid<BaseDice> board, ICollection<(int row, int col)> dropCells) {
    var work = new Queue<(int r, int c)>(dropCells);

    var dirs = new (int dr, int dc)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

    while (work.Count > 0) {
      var (startRow, startCol) = work.Dequeue();

      var startCell = board.Get(startRow, startCol);
      if (startCell == null) {
        continue;
      }

      var group = new Stack<(int, int)>();
      var visited = new bool[board.Rows, board.Cols];

      void DFS(int r, int c) {
        visited[r, c] = true;
        group.Push((r, c));

        foreach (var (dr, dc) in dirs) {
          int nextRow = r + dr, nextCol = c + dc;
          if (!board.IsInBounds(nextRow, nextCol) || visited[nextRow, nextCol]) {
            continue;
          }

          var dice = board.Get(nextRow, nextCol);
          if (dice != null && dice.Type == startCell.Type) {
            DFS(nextRow, nextCol);
          }
        }
      }

      DFS(startRow, startCol);

      if (group.Count >= _numberCellMatching) {
        while (group.Count > 0) {
          var (row, col) = group.Pop();
          if (!board.IsEmpty(row, col)) {
            board.Clear(row, col);
          }
        }

        DiceType? next = startCell.GetNextDiceType();
        if (next.HasValue) {
          var upgraded = DiceSpawner.Instance.SpawnByType(next.Value);
          board.Set(startRow, startCol, upgraded);
          work.Enqueue((startRow, startCol));
        }
      }
    }
  }
}
