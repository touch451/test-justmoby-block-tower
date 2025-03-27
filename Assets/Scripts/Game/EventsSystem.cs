using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventsSystem
{
    #region Block Events

    public UnityEvent<PointerEventData> onBlockBeginDrag = new UnityEvent<PointerEventData>();
    public UnityEvent<PointerEventData> onBlockEndDrag = new UnityEvent<PointerEventData>();
    public UnityEvent<PointerEventData> onBlockDrag = new UnityEvent<PointerEventData>();

    public UnityEvent<Block> onBlockDragOutFromScroll = new UnityEvent<Block>();
    public UnityEvent<Block> onBlockDestroyed = new UnityEvent<Block>();

    #endregion

    public void RemoveAllListeners()
    {
        onBlockBeginDrag.RemoveAllListeners();
        onBlockEndDrag.RemoveAllListeners();
        onBlockDrag.RemoveAllListeners();

        onBlockDragOutFromScroll.RemoveAllListeners();
        onBlockDestroyed.RemoveAllListeners();
    }
}
