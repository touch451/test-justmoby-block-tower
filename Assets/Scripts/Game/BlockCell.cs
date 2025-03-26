using UnityEngine;

public class BlockCell : MonoBehaviour
{
    private Block _block = null;

    public void SetBlock(BlockColor color)
    {
        if (_block != null)
            Destroy(_block);

        _block = BlockFactory.Instance.InstantiateBlock(color, transform);
    }
}
