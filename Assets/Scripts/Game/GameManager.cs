using UnityEngine;

public class GameManager : MonoBehaviour {
  public static GameManager Instance { get; private set; }
  public GameData GameData { get; private set; }
  private Canvas canvas;


  [SerializeField] private GameObject DiceInputGO;

  private void Awake() {
    GameData = GetComponent<GameData>();

    if (canvas == null)
      canvas = GetComponentInParent<Canvas>();
    if (canvas == null)
      canvas = FindFirstObjectByType<Canvas>();
    if (canvas == null)
      Debug.LogError("No Canvas found for DiceInput.", this);

    if (Instance != null && Instance != this) {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
  }

  private void Start() {
    Initialze();
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.RightArrow)) {
      GameData.CurrentDiceInput.RotateRight();
      Debug.Log("Rotate right successfully!");
    }

    if (Input.GetKeyDown(KeyCode.LeftArrow)) {
      GameData.CurrentDiceInput.RotateLeft();
      Debug.Log("Rotate left successfully!");
    }
  }

  public void Initialze() {
    GameData.CurrentDiceInput = GenerateIncomingDice();
    GameData.CurrentDiceInput.UnlockForDrag();

    GameData.NextDiceInput = GenerateIncomingDice();

    GameData.CurrentDiceInput.GetComponent<RectTransform>().anchoredPosition = GameData.CurrentDiceInputGOPosition;
    GameData.NextDiceInput.GetComponent<RectTransform>().anchoredPosition = GameData.NextDiceInputGOPosition;
  }

  public void GenerateNewInput() {
    GameData.CurrentDiceInput = GameData.NextDiceInput;
    GameData.CurrentDiceInput.UnlockForDrag();

    GameData.NextDiceInput = GenerateIncomingDice();

    GameData.CurrentDiceInput.GetComponent<RectTransform>().anchoredPosition = GameData.CurrentDiceInputGOPosition;
    GameData.NextDiceInput.GetComponent<RectTransform>().anchoredPosition = GameData.NextDiceInputGOPosition;
  }

  private IncomingDice GenerateIncomingDice() {
    IncomingDice incomingDice = IncomingDiceManager.Instance.RandomIncomingDice();
    incomingDice.LockForDrag();
    incomingDice.OnDropSuccessful += GenerateNewInput;
    return incomingDice;
  }
}
