using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPanel : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private ScrollCell cellPrefab;
    [SerializeField] private ScrollRect scroll;

    private List<ScrollCell> cells = new List<ScrollCell>();
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

    public void SetBlocks(int count, List<BlockColor> colors)
    {
        int colorIndex = 0;

        for (int i = 0; i < count; i++)
        {
            if (colorIndex >= colors.Count)
                colorIndex = 0;

            var blockColor = colors[colorIndex];
            var cell = Instantiate(cellPrefab, scroll.content);

            cell.name += "_" + i;
            cell.SetBlock(blockColor);
            cells.Add(cell);

            colorIndex++;
        }
    }
}
