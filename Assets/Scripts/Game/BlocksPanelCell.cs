using DG.Tweening;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BlocksPanelCell : MonoBehaviour
{
    [SerializeField] private ScrollRectNested scroll;
    [SerializeField] private Block blockPrefab;

    public BlockColor blockColor { get; private set; } = BlockColor.None;

    private AsyncOperationHandle<Sprite> texHandle;

    private void OnDestroy()
    {
        AddressablesUtils.ReleaseAsset(texHandle);
        scroll.onValueChanged.RemoveListener(OnScrollValueChanged);
    }

    public void Init(BlockColor color)
    {
        blockColor = color;
        LoadBlockTexture();

        scroll.onValueChanged.AddListener(OnScrollValueChanged);
    }

    private void OnScrollValueChanged(Vector2 value)
    {
        Debug.LogWarning(scroll.velocity);
    }

    private void LoadBlockTexture()
    {
        string assetKey = Pathes.GetAddressablesKeyToAsset(blockColor);

        StartCoroutine(AddressablesUtils.LoadAddressableAsset_Co<Sprite>(
            assetKey, OnLoaded, OnFailed));

        void OnLoaded(AsyncOperationHandle<Sprite> handle)
        {
            texHandle = handle;
            CreateBlock(withAnimation: false);
        }

        void OnFailed()
        {
            Debug.LogError("Error load block texture by key: " + assetKey);
        }
    }

    private void CreateBlock(bool withAnimation)
    {
        var block = Instantiate(blockPrefab, scroll.content);
        block.transform.localPosition = new Vector3(0, 0, -1);
        block.SetBlockSprite(texHandle.Result);

        if (withAnimation)
        {
            block.transform.localScale = Vector3.zero;
            block.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }
}
