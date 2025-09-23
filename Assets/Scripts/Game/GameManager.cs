using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public static GameManager Instance { get; private set; }


  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
  }

  private void Update() {
    // if (Input.GetKeyDown(KeyCode.RightArrow)) {
    //   GameData.CurrentDiceInput.RotateRight();
    //   Debug.Log("Rotate right successfully!");
    // }

    // if (Input.GetKeyDown(KeyCode.LeftArrow)) {
    //   GameData.CurrentDiceInput.RotateLeft();
    //   Debug.Log("Rotate left successfully!");
    // }
  }

  // public void Initialze() {
  //   GameData.CurrentDiceInput = GenerateIncomingDice();
  //   GameData.CurrentDiceInput.UnlockDrag();

  //   GameData.NextDiceInput = GenerateIncomingDice();

  //   GameData.CurrentDiceInput.GetComponent<RectTransform>().anchoredPosition = GameData.CurrentDiceInputGOPosition;
  //   GameData.NextDiceInput.GetComponent<RectTransform>().anchoredPosition = GameData.NextDiceInputGOPosition;
  // }

  // public void GenerateNewInput() {
  //   GameData.CurrentDiceInput = GameData.NextDiceInput;
  //   GameData.CurrentDiceInput.UnlockDrag();

  //   GameData.NextDiceInput = GenerateIncomingDice();

  //   GameData.CurrentDiceInput.GetComponent<RectTransform>().anchoredPosition = GameData.CurrentDiceInputGOPosition;
  //   GameData.NextDiceInput.GetComponent<RectTransform>().anchoredPosition = GameData.NextDiceInputGOPosition;
  // }

  // private IncomingDice GenerateIncomingDice() {
  //   IncomingDice incomingDice = IncomingDiceManager.Instance.RandomIncomingDiceGrid();
  //   incomingDice.LockDrag();
  //   incomingDice.OnDropSuccessful += GenerateNewInput;
  //   return incomingDice;
  // }

  //   public void HandleAfterDrop(List<(int, int)> incomingDices) {
  //     var dices = GameData.DiceSlotGrid.Dices;
  //     int rows = dices.GetLength(0);
  //     int cols = dices.GetLength(1);

  //     int[] dx = { -1, 0, 0, 1 };
  //     int[] dy = { 0, -1, 1, 0 };
  //     bool[,] vst = new bool[rows, cols];

  //     bool IsInBounds(int r, int c) => 0 <= r && r < rows && 0 <= c && c < cols;

  //     Stack<(int, int)> st = new();

  //     void Dfs(int r, int c) {
  //       Debug.Log($"DFS: [{r}c {c}]");
  //       vst[r, c] = true;
  //       st.Push((r, c));
  //       for (int i = 0; i < 4; ++i) {
  //         int newRow = r + dx[i];
  //         int newCol = c + dy[i];

  //         if (!IsInBounds(newRow, newCol)) {
  //           Debug.Log($"DFS: [{r} {c}] - IsInBounds");
  //           continue;
  //         }

  //         if (vst[newRow, newCol]) {
  //           Debug.Log($"DFS: [{r} {c}] - (vst[newRow, newCol])");
  //           continue;
  //         }

  //         if (dices[newRow, newCol] == null) {
  //           Debug.Log($"DFS: [{r} {c}] - (dices[newRow, newCol] == null)");
  //           continue;
  //         }

  //         if (dices[newRow, newCol].Type != dices[r, c].Type) {
  //           Debug.Log($"DFS: [{r} {c}] - dices[newRow, newCol].Type != dices[r, c].Type");
  //           continue;
  //         }

  //         Dfs(newRow, newCol);
  //       }
  //     }

  //     foreach (var cell in incomingDices) {
  //       Debug.Log($"Start at [{cell.Item1}, {cell.Item2}]");
  //       if (vst[cell.Item1, cell.Item2])
  //         continue;

  //       DiceType targetType = GameData.DiceSlotGrid.Dices[cell.Item1, cell.Item2].Type;
  //       Dfs(cell.Item1, cell.Item2);

  //       if (st.Count >= 3) {
  //         Debug.Log($"========= Start with type {targetType} at [{cell.Item1}, {cell.Item2}]=========");
  //         while (st.Count > 0) {
  //           (int r, int c) = st.Peek();
  //           Debug.Log($"[{r}, {c}] -> {dices[r, c]}");
  //           GameData.AddScore(dices[r, c].Value);
  //           GameData.DiceSlotGrid.Slots[r, c].RemoveDice();
  //           st.Pop();
  //         }

  //         var nextDice = DiceManager.Instance.GetNextDice(targetType);
  //         if (nextDice != null) {
  //           GameData.DiceSlotGrid.Slots[cell.Item1, cell.Item2].PlaceDice(nextDice);
  //         }
  //         Debug.Log($"========= End =========");
  //       }

  //       st.Clear();
  //     }
  //   }
}
