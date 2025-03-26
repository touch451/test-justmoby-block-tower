using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlocksPanel : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private BlocksPanelCell cellPrefab;
    [SerializeField] private ScrollRect scroll;

    private List<BlocksPanelCell> cells = new List<BlocksPanelCell>();

    private void Start()
    {
        CreateCells();
    }

    private void CreateCells()
    {
        var blocksInLevel = levelConfig.GetBlocks();

        for (int i = 0; i < blocksInLevel.Count; i++)
        {
            var blockColor = blocksInLevel[i];
            var cell = Instantiate(cellPrefab, scroll.content);

            cell.Init(blockColor);
            cells.Add(cell);
        }
    }
}
