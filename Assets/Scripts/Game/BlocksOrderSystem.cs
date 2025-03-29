using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksOrderSystem
{
    private List<Block> orderBlocks = new List<Block>();

    public int maxIndex => orderBlocks.Count - 1;
    public bool hasInstalledBlocks => orderBlocks.Count > 0;

    public void AddBlock(Block block)
    {
        if (ContainsBlock(block))
            RemoveBlock(block);

        orderBlocks.Add(block);
    }

    public void RemoveBlock(Block block)
    {
        orderBlocks.Remove(block);
    }

    public int GetBlockOrderIndex(Block block)
    {
        return orderBlocks.IndexOf(block);
    }

    public bool ContainsBlock(Block block)
    {
        return orderBlocks.Contains(block);
    }

    public List<Block> GetUpperBlocks(Block block)
    {
        List<Block> result = new List<Block>();

        if (ContainsBlock(block))
        {
            int blockIndex = GetBlockOrderIndex(block);
            int nextIndex = blockIndex + 1;
            int count = orderBlocks.Count - nextIndex;
            result = orderBlocks.GetRange(nextIndex, count);
        }

        return result;
    }

    public Block GetUpperBlock()
    {
        return orderBlocks[orderBlocks.Count - 1];
    }
}
