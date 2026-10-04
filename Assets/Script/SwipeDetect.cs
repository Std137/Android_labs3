using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetect: MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Камера")]
    public TouchRotate cameraOrbit;
    private int activeId = -1;

    public void OnBeginDrag(PointerEventData eventData) => activeId = eventData.pointerId;

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != activeId) return;
        cameraOrbit.RotateCamera(eventData.delta.x, eventData.delta.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == activeId) activeId = -1;
    }
}