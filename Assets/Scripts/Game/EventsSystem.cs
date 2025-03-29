using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventsSystem
{
    #region Block Events

    public UnityEvent<Block, PointerEventData> onBlockBeginDrag = new UnityEvent<Block, PointerEventData>();
    public UnityEvent<PointerEventData> onBlockEndDrag = new UnityEvent<PointerEventData>();
    public UnityEvent<PointerEventData> onBlockDrag = new UnityEvent<PointerEventData>();

    public UnityEvent<Block> onBlockDragOutFromScroll = new UnityEvent<Block>();
    public UnityEvent<Block> onBlockDestroy = new UnityEvent<Block>();
    public UnityEvent<Block> onBlockInstall = new UnityEvent<Block>();

    #endregion

    public void RemoveAllListeners()
    {
        onBlockBeginDrag.RemoveAllListeners();
        onBlockEndDrag.RemoveAllListeners();
        onBlockDrag.RemoveAllListeners();

        onBlockDragOutFromScroll.RemoveAllListeners();
        onBlockDestroy.RemoveAllListeners();
    }
}
