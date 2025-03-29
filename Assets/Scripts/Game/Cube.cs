using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Cube : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;

    private AsyncOperationHandle<Sprite> _texHandle;

    private bool isFall;
    private float fallSpeed;

    public BlockColor color { get; private set; } = BlockColor.None;
    public bool inScroll { get; private set; } = false;
    public Bounds bounds => boxCollider.bounds;
    
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

        spriteRenderer.sortingOrder = 1;
        GameManager.Instance.events.onBlockBeginDrag?.Invoke(this, eventData);
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

    public void DoFall(float destroyPosY, float delay = 0f)
    {
        isFall = true;

        float towerHeight = GameManager.Instance.GetTowerHeight();
        float fallDistance = Mathf.Abs(bounds.min.y - destroyPosY);
        float targetPosY = destroyPosY;
        bool hasBlockBelow = HasBlockBelow(out float distanceToBlock);
        bool canInstall = false;

        if (towerHeight < bounds.min.y && hasBlockBelow)
        {
            fallDistance = distanceToBlock;
            targetPosY = transform.position.y - distanceToBlock;
            canInstall = true;
        }

        float tweenTime = fallDistance / fallSpeed;
        
        transform.DOKill();

        transform
            .DOMoveY(targetPosY, tweenTime)
            .SetEase(Ease.InCubic)
            .SetDelay(delay)
            .OnComplete(() =>
            {
                if (canInstall)
                    Install(true);
                else
                    DestroyBlock();
            });
    }

    private void StopFall()
    {
        isFall = false;
        transform.DOKill(true);
    }

    public void Install(bool withJump)
    {
        StopFall();

        if (withJump)
            DoJump();

        spriteRenderer.sortingOrder = 0;
        GameManager.Instance.events.onBlockInstall?.Invoke(this);
    }

    private void DoJump()
    {
        //Vector3 targetPos = transform.position += new Vector3(0, .5f, 0);

        transform.DOKill();
        transform.DOJump(transform.position, 0.25f, 1, 0.25f);
    }

    private bool HasBlockBelow(out float distanceToBlock)
    {
        distanceToBlock = 0f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
        
        if (!hit || hit.collider.tag != Constants.TAG_BLOCK)
            return false;

        if (!hit.collider.TryGetComponent(out Cube belowBlock))
            return false;

        if (belowBlock.inScroll)
            return false;

        distanceToBlock = Mathf.Abs(hit.point.y - bounds.min.y);
        return true;
    }

    public void DestroyBlock()
    {
        SplashSpawner.Instance.Spawn(color, transform.position);
        GameManager.Instance.events.onBlockDestroy?.Invoke(this);
        
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
