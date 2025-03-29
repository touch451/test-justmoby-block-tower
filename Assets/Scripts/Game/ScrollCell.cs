using DG.Tweening;
using UnityEngine;

public class ScrollCell : MonoBehaviour
{
    private Block block = null;
    private BlockColor color = BlockColor.None;

    private void Start()
    {
        GameManager.Instance.events.onBlockDestroy.AddListener(OnBlockDestroy);
        GameManager.Instance.events.onBlockInstall.AddListener(OnBlockInstall);
    }

    public void SetBlock(BlockColor color)
    {
        this.color = color;
        float fallSpeed = GameManager.Instance.config.FallSpeed;

        block = BlockFactory.Instance.GetBlock(color, fallSpeed, true, transform);
        block.transform.localPosition = Vector3.back;
        block.name += "_" + color.ToString();
    }

    private void OnBlockDestroy(Block destroyedBlock)
    {
        if (destroyedBlock == block)
            CreateNewBlock();
    }

    private void OnBlockInstall(Block installedBlock)
    {
        if (installedBlock == block)
            CreateNewBlock();
    }

    private void CreateNewBlock()
    {
        SetBlock(color);

        block.transform.localScale = Vector3.zero;
        block.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }
}
