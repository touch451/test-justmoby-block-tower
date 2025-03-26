using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPanel : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private BlockCell cellPrefab;
    [SerializeField] private ScrollRect scroll;

    private List<BlockCell> _cells = new List<BlockCell>();
    private Bounds _worldBounds = new Bounds();

    public ScrollRect Scroll => scroll;
    public Bounds WorldBounds => _worldBounds;

    private void Start()
    {
        CalculateWorldBounds();
    }

    private void CalculateWorldBounds()
    {
        _worldBounds.SetMinMax(
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
            _cells.Add(cell);
        }
    }
}
