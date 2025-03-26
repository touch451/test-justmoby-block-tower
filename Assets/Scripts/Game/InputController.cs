using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    [SerializeField] private ScrollPanel scrollPanel;

    private Vector2 draggOffset = Vector2.zero;
    private Block draggedBlock = null;

    private void Start()
    {
        SetListeners();
    }

    private void SetListeners()
    {
        GameManager.Instance.events.onBlockBeginDrag.AddListener(OnBlockBeginDrag);
        GameManager.Instance.events.onBlockEndDrag.AddListener(OnBlockEndDrag);
        GameManager.Instance.events.onBlockDrag.AddListener(OnBlockDrag);
    }

    private void OnBlockBeginDrag(PointerEventData eventData)
    {
        if (draggedBlock != null)
            Destroy(draggedBlock);

        if (!eventData.pointerDrag.TryGetComponent<Block>(out draggedBlock))
            return;

        if (draggedBlock.inScroll)
        {
            bool isDragByHorizontal = Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y);

            // Если перетаскиваемый блок находится в скролл панеле и его тащут по горизонтали
            if (isDragByHorizontal)
            {
                // то заменяем объект в eventData на скролл нижней панели, чтобы двигать скролл, а не блок.
                var scrollGO = scrollPanel.Scroll.gameObject;

                eventData.pointerDrag = scrollGO;
                ExecuteEvents.ExecuteHierarchy(scrollGO, eventData, ExecuteEvents.beginDragHandler);
                draggedBlock = null;
            }
            else
            {
                Vector3 touchWorldPos = eventData.pointerCurrentRaycast.worldPosition;
                Vector3 blockWorldPos = draggedBlock.transform.position;
                draggOffset = blockWorldPos - touchWorldPos;
            }
        }
    }

    private void OnBlockDrag(PointerEventData eventData)
    {
        if (draggedBlock == null)
            return;

        Vector3 touchWorldPos = eventData.pointerCurrentRaycast.worldPosition;
        Vector3 blockWorldPos = draggedBlock.transform.position;

        float x = draggedBlock.inScroll ? blockWorldPos.x : touchWorldPos.x + draggOffset.x;

        Vector3 targetPosition =
            new Vector3(x, touchWorldPos.y + draggOffset.y, blockWorldPos.z);

        draggedBlock.transform.position = targetPosition;

        if (draggedBlock.inScroll && targetPosition.y > scrollPanel.WorldBounds.max.y)
            draggedBlock.OnPullOutFromScroll();
    }

    private void OnBlockEndDrag(PointerEventData eventData)
    {
        if (draggedBlock.inScroll)
            draggedBlock.ReturnToCenterCell();

        draggedBlock = null;
    }
}
