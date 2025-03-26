using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Block : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private AsyncOperationHandle<Sprite> _texHandle;

    public BlockColor color { get; private set; } = BlockColor.None;
    public bool inScroll { get; private set; } = true;

    private void OnDestroy()
    {
        AddressablesTools.ReleaseAsset(_texHandle);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.Instance.events.onBlockBeginDrag?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        GameManager.Instance.events.onBlockDrag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.Instance.events.onBlockEndDrag?.Invoke(eventData);
    }

    public void OnPullOutFromScroll()
    {
        inScroll = false;
        transform.SetParent(null, true);

        GameManager.Instance.events.onBlockPullOutFromScroll?.Invoke(this);
    }

    public void ReturnToCenterCell()
    {
        if (inScroll)
        {
            transform.DOKill();
            transform.DOLocalMove(new Vector3(0, 0, transform.localPosition.z), 0.25f);
        }
    }

    public void SetColor(BlockColor color)
    {
        if (this.color == color && _texHandle.IsValid())
        {
            spriteRenderer.sprite = _texHandle.Result;
        }
        else
        {
            LoadColorTexture(color);
        }
    }

    private void LoadColorTexture(BlockColor color)
    {
        string assetKey = Pathes.GetAddressablesKeyToAsset(color);

        StartCoroutine(AddressablesTools.LoadAddressableAsset_Co<Sprite>(
            assetKey, OnLoaded, OnFailed));

        void OnLoaded(AsyncOperationHandle<Sprite> handle)
        {
            _texHandle = handle;
            spriteRenderer.sprite = _texHandle.Result;
        }

        void OnFailed()
        {
            Debug.LogError("Error load block texture by key: " + assetKey);
        }
    }
}
