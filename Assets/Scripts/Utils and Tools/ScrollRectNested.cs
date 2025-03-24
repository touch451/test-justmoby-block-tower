using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollRectNested : ScrollRect
{
    GameObject RerouteTarget;

    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        // Find potential reroute target in our parents.
        RerouteTarget = ExecuteEvents.GetEventHandler<IDragHandler>(transform.parent.gameObject);

        // If a reroute target is available, dispatch this event to it.
        if (RerouteTarget != null)
            ExecuteEvents.ExecuteHierarchy(RerouteTarget, eventData, ExecuteEvents.initializePotentialDrag);

        // Dispatch to base class.
        base.OnInitializePotentialDrag(eventData);
    }

    public override void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        // Check if this drag event is off axis from what this scroll rect supports.
        var offaxis = !horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y)
            || !vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y);

        // If the drag is off axis and a reroute object is available, then update the event handler and redispatch.
        // Otherwise dispatch to base class.
        if (RerouteTarget != null && offaxis)
        {
            eventData.pointerDrag = RerouteTarget;
            ExecuteEvents.ExecuteHierarchy(RerouteTarget, eventData, ExecuteEvents.beginDragHandler);
        }
        else
        {
            base.OnBeginDrag(eventData);
        }

        // Clear out reroute target.
        RerouteTarget = null;
    }
}
