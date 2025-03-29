using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour
{
    [SerializeField] private ScrollPanel scrollPanel;

    private Vector2 touchOffset = Vector2.zero;
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

    private void OnBlockBeginDrag(Block block, PointerEventData eventData)
    {
        if (draggedBlock != null)
            return;

        draggedBlock = block;

        Vector3 touchWorldPos = eventData.pointerCurrentRaycast.worldPosition;
        Vector3 blockWorldPos = draggedBlock.transform.position;
        touchOffset = blockWorldPos - touchWorldPos;

        if (draggedBlock.inScroll)
        {
            bool isDragByHorizontal = Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y);

            // Если перетаскиваемый блок находится в скролл панеле и его тащут по горизонтали,
            if (isDragByHorizontal)
            {
                // то заменяем ссылку на объект в eventData на скролл, чтобы начать двигать скролл, а не блок.
                var scrollGO = scrollPanel.Scroll.gameObject;

                eventData.pointerDrag = scrollGO;
                ExecuteEvents.ExecuteHierarchy(scrollGO, eventData, ExecuteEvents.beginDragHandler);
                draggedBlock = null;
            }
        }
    }

    private void OnBlockDrag(PointerEventData eventData)
    {
        if (draggedBlock == null)
            return;

        Vector3 touchWorldPos = eventData.pointerCurrentRaycast.worldPosition;
        Vector3 blockWorldPos = draggedBlock.transform.position;

        // Если блок в скролл панеле, то двигаем его только по вертикали
        float x = draggedBlock.inScroll ? blockWorldPos.x : touchWorldPos.x + touchOffset.x;

        Vector3 targetPosition =
            new Vector3(x, touchWorldPos.y + touchOffset.y, blockWorldPos.z);

        draggedBlock.transform.position = targetPosition;

        // Если блок находился в скролл панеле и его вытянули выше верхней границы панели
        if (draggedBlock.inScroll && targetPosition.y > scrollPanel.WorldBounds.max.y)
            draggedBlock.OnDragOutFromScroll();
    }

    private void OnBlockEndDrag(PointerEventData eventData)
    {
        if (draggedBlock == null)
            return;

        if (draggedBlock.inScroll)
        {
            // Если блок отпустили не вытащив из скролл панели, то возвращаем его на свое место
            draggedBlock.ReturnToCell();
        }
        else
        {
            Vector3 blockScreenPosition =
                Camera.main.WorldToScreenPoint(draggedBlock.transform.position);

            bool isBlockInLeftScreenArea = blockScreenPosition.x < Screen.width / 2f;
            bool hasInstalledBlocks = GameManager.Instance.blocksOrder.hasInstalledBlocks;

            if (!hasInstalledBlocks && !isBlockInLeftScreenArea)
            {
                // Если это самый первый блок, то сразу его устанавливаем.
                draggedBlock.Install(false);
            }
            else
            {
                // Если это не первый блок, то он начинает падать.
                draggedBlock.DoFall();
            }
        }
            
        draggedBlock = null;
    }
}
