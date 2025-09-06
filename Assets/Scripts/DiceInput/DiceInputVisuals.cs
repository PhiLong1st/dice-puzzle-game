using UnityEngine;
using UnityEngine.UI;

public class DiceInputVisuals : MonoBehaviour
{
  [SerializeField] private GameObject diceContainer;
  [SerializeField] private GameObject visual;

  private GridLayoutGroup gridLayoutGroup;
  private CanvasRenderer canvasRenderer;

  private void Start()
  {
    gridLayoutGroup = diceContainer.gameObject.AddComponent<GridLayoutGroup>();
    gridLayoutGroup.cellSize = new Vector2(100f, 100f);
    gridLayoutGroup.spacing = new Vector2(5f, 5f);
    gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
    gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
    gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
    gridLayoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;

    canvasRenderer = visual.gameObject.AddComponent<CanvasRenderer>();
    canvasRenderer.cullTransparentMesh = true;
  }
}