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
                touchOffset = blockWorldPos - touchWorldPos;
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
        if (draggedBlock.inScroll)
        {
            // Если блок отпустили не вытащив из скролл панели, то возвращаем его на свое место
            draggedBlock.ReturnToCell();
        }
        else
        {
            // Если блок отпустили на поле, то он начинает падение.
            // Блок уничтожится, когда достигнет Y позиции немного выше скролл панели =
            // верхняя граница скролл панели + еще 25% от ее высоты.
            float scrollSizeY = scrollPanel.WorldBounds.size.y;
            float yPositionToDestroyBlock = scrollPanel.WorldBounds.max.y + scrollSizeY * 0.25f;
            draggedBlock.DoFall(yPositionToDestroyBlock);
        }
            
        draggedBlock = null;
    }
}
