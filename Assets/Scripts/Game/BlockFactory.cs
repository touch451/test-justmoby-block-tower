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

    public Block InstantiateBlock(BlockColor color)
    {
        Block block = Instantiate(blockPrefab);
        block.SetColor(color);
        return block;
    }

    public Block InstantiateBlock(BlockColor color, Transform parent)
    {
        Block block = Instantiate(blockPrefab, parent);
        block.SetColor(color);
        return block;
    }
}
