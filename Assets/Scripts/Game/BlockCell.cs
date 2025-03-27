using DG.Tweening;
using UnityEngine;

public class BlockCell : MonoBehaviour
{
    private Block block = null;
    private BlockColor color = BlockColor.None;

    private void Start()
    {
        GameManager.Instance.events.onBlockDestroyed.AddListener(OnBlockDestroyed);
    }

    public void SetBlock(BlockColor color)
    {
        this.color = color;
        float fallSpeed = GameManager.Instance.LevelConfig.FallSpeed;

        block = BlockFactory.Instance.InstantiateBlock(color, fallSpeed, true, transform);
        block.transform.localPosition = Vector3.back;
    }

    private void OnBlockDestroyed(Block destroyedBlock)
    {
        if (destroyedBlock == block)
            CreateNewBlock();
    }

    private void CreateNewBlock()
    {
        SetBlock(color);

        block.transform.localScale = Vector3.zero;
        block.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }
}
