using System;
using System.Collections.Generic;
using UnityEngine;

public enum DiceType { Zero = 0, One, Two, Three, Four, Five, Six };

[Serializable]
public struct DicePrefab
{
  public DiceType Type;
  public GameObject Prefab;
}

public class DiceManager : MonoBehaviour
{
  public static DiceManager Instance { get; private set; }

  [SerializeField] private DicePrefab[] dicePrefabs;
  private Dictionary<DiceType, GameObject> cachedDices;

  [SerializeField] private GameObject defaultDiceGameObject;
  public Dice DefaultDice => defaultDiceGameObject.GetComponent<Dice>();

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
    BuildCachedDices();
  }

  private void BuildCachedDices()
  {
    cachedDices = new();
    Debug.Log("<color=#60A5FA>DICE MANAGER: Loading dice prefabs…</color>");
    foreach (var dicePrefab in dicePrefabs)
    {
      if (cachedDices.ContainsKey(dicePrefab.Type)) continue;

      if (dicePrefab.Prefab == null)
      {
        Debug.Log($"<color=#60A5FA>DICE MANAGER: Missing Prefab for ({dicePrefab.Type}).", this);
        continue;
      }
      cachedDices.Add(dicePrefab.Type, dicePrefab.Prefab);
    }
    Debug.Log("<color=#60A5FA>DICE MANAGER: Done.</color>");
  }

  public GameObject GetDicePrefab(DiceType diceType)
  {
    cachedDices.TryGetValue(diceType, out GameObject dicePrefab);

    if (dicePrefab == null)
    {
      Debug.LogError($"DiceManager: No prefab registered for {diceType}.", this);
    }

    var clonedPrefab = Instantiate(dicePrefab);
    return clonedPrefab;
  }

  public DiceType RandomDiceType() => (DiceType)UnityEngine.Random.Range(1, 7);
}