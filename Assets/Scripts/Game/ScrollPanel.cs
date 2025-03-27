using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPanel : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private BlockCell cellPrefab;
    [SerializeField] private ScrollRect scroll;

    private List<BlockCell> cells = new List<BlockCell>();
    private Bounds worldBounds = new Bounds();

    public Bounds WorldBounds => worldBounds;
    public ScrollRect Scroll => scroll;

    private void Start()
    {
        CalculateWorldBounds();
    }

    private void CalculateWorldBounds()
    {
        worldBounds.SetMinMax(
            ScriptTools.GetRectWorldOffsetMin(rect),
            ScriptTools.GetRectWorldOffsetMax(rect));
    }

    public void SetBlocks(List<BlockColor> colors)
    {
        for (int i = 0; i < colors.Count; i++)
        {
            var blockColor = colors[i];
            var cell = Instantiate(cellPrefab, scroll.content);

            cell.SetBlock(blockColor);
            cells.Add(cell);
        }
    }
}
