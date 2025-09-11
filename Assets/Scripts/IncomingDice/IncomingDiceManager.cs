using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class IncomingDiceManager : MonoBehaviour
{
  public static IncomingDiceManager Instance { get; private set; }

  [SerializeField] private GameObject IncomingDiceGO;
  private RectTransform rectTransform;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    rectTransform = GetComponent<RectTransform>();
    DontDestroyOnLoad(gameObject);
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      RandomDiceInput();
      Debug.Log("Random successfully!");
    }
  }

  public IncomingDice RandomDiceInput()
  {
    int[,] template = IncomingDiceTemplate.RandomTemplate();
    int rows = template.GetLength(0);
    int cols = template.GetLength(1);

    DiceType[,] diceTypes = new DiceType[rows, cols];
    for (int r = 0; r < rows; ++r)
    {
      for (int c = 0; c < cols; ++c)
      {
        diceTypes[r, c] = (template[r, c] == 0) ? DiceType.Zero : EnumUtils.RandomEnumValue<DiceType>();
      }
    }

    IncomingDice incomingDice = Instantiate(IncomingDiceGO).GetComponent<IncomingDice>();
    incomingDice.Initialze(diceTypes);

    return incomingDice;
  }
}