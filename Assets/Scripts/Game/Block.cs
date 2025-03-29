using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Block : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
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

    public void DoFall(float distanceY = float.Epsilon, float delay = 0f)
    {
        isFall = true;

        bool canInstall = false;
        float targetY = 0f;
        float fallDistance = 0f;
        
        bool needCalculateDistance = distanceY == float.Epsilon;
        
        float destroyPosY =
            GameManager.Instance.ScrollPanel.WorldBounds.max.y + bounds.size.y / 2f;

        if (needCalculateDistance)
        {
            bool hasBlockBelow = HasBlockBelow(out float distanceToBlock);

            canInstall = IsBlockAboveTower() && hasBlockBelow;

            targetY = canInstall
                ? transform.position.y - distanceToBlock
                : destroyPosY;

            fallDistance = canInstall
                ? distanceToBlock
                : Mathf.Abs(bounds.min.y - destroyPosY);
        }
        else
        {
            targetY = transform.position.y - distanceY;
            canInstall = targetY > destroyPosY;

            if (!canInstall)
                targetY = destroyPosY;
            
            fallDistance = Mathf.Abs(bounds.min.y - targetY);
        }
        
        float tweenTime = fallDistance / fallSpeed;
        
        transform.DOKill();
        transform
            .DOMoveY(targetY, tweenTime)
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
        transform.DOKill();
        transform.DOJump(transform.position, 0.25f, 1, 0.25f);
    }

    public bool IsBlockAboveTower()
    {
        float towerHeight = GameManager.Instance.GetTowerHeight();
        return towerHeight < bounds.min.y;
    }

    public bool HasBlockBelow(out float distanceToBlock, int ignoreCount = 0)
    {
        distanceToBlock = 0f;

        List<RaycastHit2D> allHits =
            Physics2D.RaycastAll(transform.position, Vector2.down).ToList();

        if (allHits == null || allHits.Count == 0)
            return false;

        List<RaycastHit2D> blocksHits =
            allHits.FindAll(h => h.collider.tag == Constants.TAG_BLOCK); 

        if (blocksHits == null || blocksHits.Count == 0)
            return false;

        for (int i = 0; i < blocksHits.Count; i++) 
        {
            if (i < ignoreCount)
                continue;

            var hit = blocksHits[i];

            if (!hit.collider.TryGetComponent(out Block belowBlock))
                continue;

            if (belowBlock.inScroll)
                continue;

            distanceToBlock = Mathf.Abs(hit.point.y - bounds.min.y);
            return true;
        }

        return false;
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
