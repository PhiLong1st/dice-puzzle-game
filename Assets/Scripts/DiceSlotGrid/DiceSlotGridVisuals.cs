using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class DiceSlotGridVisuals : MonoBehaviour
{
  [SerializeField] private GameObject diceSlotPrefab;
  private GridLayoutGroup gridLayoutGroup;
  private CanvasRenderer canvasRenderer;
  private Image bgImage;

  private void Start()
  {
    gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
    gridLayoutGroup.cellSize = new Vector2(100f, 100f);
    gridLayoutGroup.spacing = new Vector2(5f, 5f);
    gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
    gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
    gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
    gridLayoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;

    canvasRenderer = gameObject.AddComponent<CanvasRenderer>();
    canvasRenderer.cullTransparentMesh = true;

    bgImage = gameObject.AddComponent<Image>();
    bgImage.GetComponent<Image>().color = new Color32(0, 0, 0, 105);
  }
}