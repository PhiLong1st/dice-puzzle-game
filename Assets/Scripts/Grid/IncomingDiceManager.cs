using UnityEngine;

public class IncomingDiceManager : MonoBehaviour {

  public static IncomingDiceManager Instance { get; private set; }

  [SerializeField] private GameObject incomingDicePrefab;
  private Canvas canvas;

  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(gameObject);
      return;
    }

    Instance = this;

    if (canvas == null)
      canvas = GetComponentInParent<Canvas>();
    if (canvas == null)
      canvas = FindFirstObjectByType<Canvas>();
    if (canvas == null)
      Debug.LogError("No Canvas found for DiceInput.", this);

    DontDestroyOnLoad(gameObject);
  }

  public IncomingDice RandomIncomingDiceGrid() {
    int[,] template = IncomingDiceTemplate.RandomTemplate();
    int rows = template.GetLength(0);
    int cols = template.GetLength(1);

    DiceType?[,] diceTypes = new DiceType?[rows, cols];
    for (int r = 0; r < rows; ++r) {
      for (int c = 0; c < cols; ++c) {
        diceTypes[r, c] = (template[r, c] == 0) ? null : EnumUtils.RandomEnumValue<DiceType>();
      }
    }

    var clonedPrefab = Instantiate(incomingDicePrefab);
    var incomingDice = clonedPrefab.GetComponent<IncomingDice>();
    incomingDice.GetComponent<RectTransform>().SetAsChild(canvas.GetComponent<RectTransform>());
    incomingDice.SetGridData(diceTypes);
    return incomingDice;
  }
}

