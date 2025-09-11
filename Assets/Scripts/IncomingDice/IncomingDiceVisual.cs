using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GridLayoutGroup))]
public class IncomingDiceVisual : MonoBehaviour
{
  public readonly Vector2 DiceCellSize = new Vector2(100f, 100f);
  public readonly Vector2 DiceCellSpacing = new Vector2(5f, 5f);

  private IncomingDice incomingDice;
  private GridLayoutGroup gridLayoutGroup;
  private RectTransform rectTransform;

  private void Awake()
  {
    gridLayoutGroup = GetComponent<GridLayoutGroup>();
    incomingDice = GetComponent<IncomingDice>();
    rectTransform = GetComponent<RectTransform>();
  }

  public void Initilize()
  {
    gridLayoutGroup.cellSize = DiceCellSize;
    gridLayoutGroup.spacing = DiceCellSpacing;

    gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
    gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
    gridLayoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;

    gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;

    int rows = incomingDice.IncomingDices.GetLength(0);
    int cols = incomingDice.IncomingDices.GetLength(1);

    float height = rows * DiceCellSize.x + (rows - 1) * DiceCellSpacing.x;
    float width = cols * DiceCellSize.y + (cols - 1) * DiceCellSpacing.y;

    rectTransform.sizeDelta = new Vector2(width, height);
    rectTransform.pivot = new Vector2(0.5f, 0.5f);
    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
  }
}