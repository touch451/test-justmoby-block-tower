using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Block : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private AsyncOperationHandle<Sprite> _texHandle;
    private bool isFall;
    private float fallSpeed;

    public BlockColor color { get; private set; } = BlockColor.None;
    public bool inScroll { get; private set; } = false;
    
    private void OnDestroy()
    {
        AddressablesTools.ReleaseAsset(_texHandle);
    }

    public void Init(BlockColor color, float fallSpeed, bool inScroll)
    {
        this.color = color;
        this.fallSpeed = fallSpeed;
        this.inScroll = inScroll;

        if (this.color == color && _texHandle.IsValid())
        {
            spriteRenderer.sprite = _texHandle.Result;
        }
        else
        {
            LoadColorTexture(color);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isFall)
            return;

        GameManager.Instance.events.onBlockBeginDrag?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isFall)
            return;

        GameManager.Instance.events.onBlockDrag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isFall)
            return;

        GameManager.Instance.events.onBlockEndDrag?.Invoke(eventData);
    }

    public void OnDragOutFromScroll()
    {
        inScroll = false;
        transform.SetParent(null, true);

        GameManager.Instance.events.onBlockDragOutFromScroll?.Invoke(this);
    }

    public void DoFall(float targetPositionY)
    {
        isFall = true;

        float distance = Mathf.Abs(transform.position.y - targetPositionY);
        float tweenTime = distance / fallSpeed;

        transform.DOKill();

        transform
            .DOMoveY(targetPositionY, tweenTime)
            .SetEase(Ease.InCubic)
            .OnComplete(DestroyBlock);
    }

    private void Install()
    {
        isFall = false;

        transform.DOKill();
    }

    private void DestroyBlock()
    {
        SplashSpawner.Instance.Spawn(color, transform.position);
        GameManager.Instance.events.onBlockDestroyed?.Invoke(this);
        
        Destroy(gameObject);
    }

    public void ReturnToCell()
    {
        if (inScroll)
        {
            transform.DOKill();
            transform.DOLocalMove(new Vector3(0, 0, transform.localPosition.z), 0.25f);
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
