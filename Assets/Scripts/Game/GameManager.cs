using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }
  public GameData GameData { get; private set; }

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
  }

  private void Start()
  {
    GameData = GetComponent<GameData>();
  }

  private void GenerateNextDiceInput()
  {
    var diceType = DiceManager.Instance.RandomDiceType();
    var randomDice = DiceManager.Instance.GetDicePrefab(diceType);
    var data = randomDice.GetComponent<DiceData>();
    GameData.GenerateNextDiceInputData(data);
  }

  private void OnDestroy()
  {
    if (Instance == this) Instance = null;
  }
}
