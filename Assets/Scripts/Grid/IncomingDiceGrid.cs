using UnityEngine;

public class IncomingDiceGrid : MonoBehaviour {
  private Vector2 _cellSize { get; } = new Vector2(100f, 100f);

  private GameObject _visual;
  private RectTransform _rectTransform;
  private BaseDice[,] _dices;

  private void Awake() {
    _rectTransform = GetComponent<RectTransform>();
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.P)) {
      Debug.Log("Renew");
      Renew();
    }
  }

  private void InitVisual() {
    // visual = DiceVisualSpawner.Instance.SpawnByType(GetCurrentVisualType());
    // visual.GetComponent<RectTransform>().SetParent(rectTransform, false);
  }

  public void Init(BaseDice[,] initDices) {
    int rows = initDices.GetLength(0);
    int cols = initDices.GetLength(1);

    _dices = new BaseDice[rows, cols];

    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        if (initDices[r, c] == null)
          continue;
        SetDice(r, c, initDices[r, c]);
      }
    }
  }

  private void SetDice(int r, int c, BaseDice dice) {
    _dices[r, c] = dice;

    RectTransform cellRectTransform = dice.GetComponent<RectTransform>();
    cellRectTransform.SetParent(_rectTransform, false);

    Vector2 cellPosition = new Vector2 {
      x = c * _cellSize.x + _cellSize.x * cellRectTransform.pivot.x,
      y = r * _cellSize.y + _cellSize.y * cellRectTransform.pivot.y
    };

    cellRectTransform.anchoredPosition = cellPosition;
  }

  private BaseDice GetDice(int r, int c) => _dices[r, c];

  private void Clear() {
    if (_dices == null) {
      return;
    }

    int rows = _dices.GetLength(0);
    int cols = _dices.GetLength(1);
    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        if (!_dices[r, c])
          continue;
        DiceSpawner.Instance.Release(_dices[r, c]);
        _dices[r, c] = null;
      }
    }
  }

  public void Renew() {
    Clear();
    int[,] template = IncomingDiceTemplate.RandomTemplate();
    int rows = template.GetLength(0);
    int cols = template.GetLength(1);

    _dices = new BaseDice[rows, cols];

    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        if (template[r, c] == 0)
          continue;

        BaseDice randomDice = DiceSpawner.Instance.SpawnRandom();
        SetDice(r, c, randomDice);
      }
    }
  }

  //Swap cell
  //rotate grid
}
