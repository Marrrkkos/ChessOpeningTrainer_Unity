using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestedScrollRect : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ScrollRect mainScrollRect;
    public SwipeController swipeController;

    private ScrollRect innerScrollRect;

    private bool horizontalDrag;

    private void Awake()
    {
        innerScrollRect = GetComponent<ScrollRect>();
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        mainScrollRect.OnInitializePotentialDrag(eventData);
        innerScrollRect.OnInitializePotentialDrag(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        horizontalDrag = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);

        innerScrollRect.vertical = !horizontalDrag;

        if (horizontalDrag)
            mainScrollRect.OnBeginDrag(eventData);
        else
            innerScrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (horizontalDrag)
            mainScrollRect.OnDrag(eventData);
        else
            innerScrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        innerScrollRect.vertical = true;

        if (horizontalDrag)
            swipeController.OnEndDrag(eventData);
        else
            innerScrollRect.OnEndDrag(eventData);
    }
}