using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class IncomingDiceManager : MonoBehaviour
{
  public static IncomingDiceManager Instance { get; private set; }

  [SerializeField] private GameObject IncomingDiceGO;
  private Canvas canvas;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
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

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      RandomIncomingDice();
      Debug.Log("Random successfully!");
    }
  }

  public IncomingDice RandomIncomingDice()
  {
    int[,] template = IncomingDiceTemplate.RandomTemplate();
    int rows = template.GetLength(0);
    int cols = template.GetLength(1);

    DiceType?[,] diceTypes = new DiceType?[rows, cols];
    for (int r = 0; r < rows; ++r)
    {
      for (int c = 0; c < cols; ++c)
      {
        diceTypes[r, c] = (template[r, c] == 0) ? null : EnumUtils.RandomEnumValue<DiceType>();
      }
    }

    IncomingDice incomingDice = Instantiate(IncomingDiceGO, canvas.transform).GetComponent<IncomingDice>();
    incomingDice.Initialze(diceTypes);

    return incomingDice;
  }
}