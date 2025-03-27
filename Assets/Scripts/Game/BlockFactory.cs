using UnityEngine;

public class BlockFactory : MonoBehaviour
{
    [SerializeField] private Block blockPrefab;

    public static BlockFactory Instance { get; private set; }

    private void Awake()
    {
        SetInstance();
    }

    private void SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Block InstantiateBlock(BlockColor color, Transform parent = null)
    {
        Block block = Instantiate(blockPrefab, parent);
        block.Init(color, 0f, false);
        return block;
    }

    public Block InstantiateBlock(BlockColor color, float fallSpeed, Transform parent = null)
    {
        Block block = Instantiate(blockPrefab, parent);
        block.Init(color, fallSpeed, false);
        return block;
    }

    public Block InstantiateBlock(BlockColor color, float fallSpeed, bool inScroll, Transform parent = null)
    {
        Block block = Instantiate(blockPrefab, parent);
        block.Init(color, fallSpeed, inScroll);
        return block;
    }
}
