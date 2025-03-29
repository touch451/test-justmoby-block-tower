using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventsSystem
{
    #region Block Events

    public UnityEvent<Cube, PointerEventData> onBlockBeginDrag = new UnityEvent<Cube, PointerEventData>();
    public UnityEvent<PointerEventData> onBlockEndDrag = new UnityEvent<PointerEventData>();
    public UnityEvent<PointerEventData> onBlockDrag = new UnityEvent<PointerEventData>();

    public UnityEvent<Cube> onBlockDragOutFromScroll = new UnityEvent<Cube>();
    public UnityEvent<Cube> onBlockDestroy = new UnityEvent<Cube>();
    public UnityEvent<Cube> onBlockInstall = new UnityEvent<Cube>();

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
