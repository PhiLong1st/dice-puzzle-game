using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceInput : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
  [SerializeField] private GameObject diceContainerGO;
  public Dice[,] DiceInputs { get; private set; }
  public event Action OnDropSuccessful;
  public bool IsDraggable { get; private set; }
  private Canvas canvas;
  private RectTransform rectTransform;
  private CanvasGroup canvasGroup;
  private Vector2 initialPosition;

  private void Awake()
  {
    rectTransform = GetComponent<RectTransform>();
    canvasGroup = GetComponent<CanvasGroup>();
    if (canvas == null) canvas = GetComponentInParent<Canvas>();
    if (canvas == null) canvas = FindFirstObjectByType<Canvas>();
    if (canvas == null) Debug.LogError("No Canvas found for DiceInput.", this);
  }

  private void BuildContainer()
  {
    var diceInputsMock = new DiceType[2, 2] { { DiceType.One, DiceType.One }, { DiceType.Zero, DiceType.One } };
    int rows = diceInputsMock.GetLength(0);
    int cols = diceInputsMock.GetLength(1);

    DiceInputs = new Dice[rows, cols];
    for (int r = 0; r < rows; ++r)
    {
      for (int c = 0; c < cols; ++c)
      {
        GameObject dicePrefab = DiceManager.Instance.GetDicePrefab(diceInputsMock[r, c]);
        DiceInputs[r, c] = dicePrefab.GetComponent<Dice>();

        RectTransform rectTransform = dicePrefab.GetComponent<RectTransform>();
        RectTransform containerRectTransform = diceContainerGO.GetComponent<RectTransform>();
        rectTransform.SetParent(containerRectTransform, worldPositionStays: false);
      }
    }
  }

  private void Start()
  {
    IsDraggable = false;
    UnlockForDrag();
    BuildContainer();
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

    if (!IsDraggable)
    {
      rectTransform.anchoredPosition = initialPosition;
      Debug.Log("Drop fail so comeback the initial position");
    }
    else
    {
      OnDropSuccessful?.Invoke();
      Debug.Log("Drop succesfull");
      Destroy(gameObject);
      LockForDrag();
    }
  }

  public void NotifyDropOk()
  {
    IsDraggable = true;
  }

  private void LockForDrag()
  {
    canvasGroup.blocksRaycasts = false;
    enabled = false;
  }

  private void UnlockForDrag()
  {
    IsDraggable = false;
    enabled = true;
    canvasGroup.blocksRaycasts = true;
  }
}
