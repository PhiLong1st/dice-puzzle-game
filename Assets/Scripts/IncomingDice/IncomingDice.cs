using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class IncomingDice : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
  public Dice[,] IncomingDices { get; private set; }

  private Canvas canvas;
  private RectTransform rectTransform;
  private CanvasGroup canvasGroup;

  public bool IsDropSuccessful { get; private set; }
  public event Action OnDropSuccessful;

  private Vector2 initialPosition;
  private DiceType[,] dataGrid;
  private IncomingDiceVisual visual;

  private void Awake()
  {
    visual = GetComponent<IncomingDiceVisual>();
    rectTransform = GetComponent<RectTransform>();
    canvasGroup = GetComponent<CanvasGroup>();

    if (canvas == null)
      canvas = GetComponentInParent<Canvas>();
    if (canvas == null)
      canvas = FindFirstObjectByType<Canvas>();
    if (canvas == null)
      Debug.LogError("No Canvas found for DiceInput.", this);
  }

  private void Start()
  {
    IsDropSuccessful = false;
    enabled = true;
    canvasGroup.blocksRaycasts = true;
  }

  public void Initialze(DiceType[,] diceTypes)
  {
    dataGrid = diceTypes;
    int rows = dataGrid.GetLength(0);
    int cols = dataGrid.GetLength(1);

    IncomingDices = new Dice[rows, cols];
    for (int r = 0; r < rows; ++r)
    {
      for (int c = 0; c < cols; ++c)
      {
        GameObject dicePrefab = DiceManager.Instance.GetDicePrefab(dataGrid[r, c]);
        IncomingDices[r, c] = dicePrefab.GetComponent<Dice>();

        RectTransform prefabRect = dicePrefab.GetComponent<RectTransform>();
        prefabRect.SetParent(rectTransform, worldPositionStays: false);
      }
    }

    visual.Initilize();

    OnDropSuccessful += GameManager.Instance.GenerateNewInput;
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    initialPosition = rectTransform.anchoredPosition;
    canvasGroup.blocksRaycasts = false;
    canvasGroup.alpha = .6f;
  }

  public void OnDrag(PointerEventData eventData)
  {
    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    canvasGroup.blocksRaycasts = true;
    canvasGroup.alpha = 1f;

    if (IsDropSuccessful)
    {
      Debug.Log("Drop succesfull");
      OnDropSuccessful?.Invoke();
      Destroy(gameObject);
    }
    else
    {
      Debug.Log("Drop fail so comeback the initial position");
      rectTransform.anchoredPosition = initialPosition;
    }
  }

  public void NotifyDropSuccessful()
  {
    IsDropSuccessful = true;
  }
}
