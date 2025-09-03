using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }

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

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      var diceGO = DiceInputManager.Instance.CreateRandomDiceInput();
      if (diceGO == null)
      {
        Debug.LogWarning("Failed to generate a random dice input.");
        return;
      }

      var diceInput = diceGO.GetComponent<DiceInputData>();
      Debug.Log($"Dice input type: {diceInput.Type}");
    }
  }

  private void OnDestroy()
  {
    if (Instance == this) Instance = null;
  }
}
