using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class SwipeController : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
   public ScrollRect scrollRect;
    public RectTransform content;
    public UIButtonController uIButtonController;
    [Header("Swipe Settings")]
    public float snapSpeed = 15f;
    public float swipeThreshold = 50f;

    private int totalPages;
    private float[] pagePositions;
    private int currentPage = 0;
    private Vector2 dragStartPosition;

    void Start()
    {
        UpdatePages();
    }

    public void UpdatePages()
    {
        totalPages = content.childCount;
        pagePositions = new float[totalPages];
        
        for (int i = 0; i < totalPages; i++)
        {
            pagePositions[i] = totalPages <= 1 ? 0 : (float)i / (totalPages - 1);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StopAllCoroutines();
        
        dragStartPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float swipeDistance;

        if (scrollRect.horizontal)
        {
            swipeDistance = dragStartPosition.x - eventData.position.x;
        }
        else
        {
            swipeDistance = dragStartPosition.y - eventData.position.y;
        }

        if (swipeDistance > swipeThreshold && currentPage < totalPages - 1)
        {
            currentPage++;
        }
        else if (swipeDistance < -swipeThreshold && currentPage > 0)
        {
            currentPage--;
        }

        ScrollToPage(currentPage);
    }

    public void ScrollToPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= totalPages) return;

        currentPage = pageIndex;
        StopAllCoroutines();
        StartCoroutine(SmoothSnap(pagePositions[currentPage]));
        uIButtonController.SetIndex(currentPage);
    }

    private IEnumerator SmoothSnap(float target)
    {
        scrollRect.velocity = Vector2.zero;

        float currentPos = scrollRect.horizontal ? scrollRect.horizontalNormalizedPosition : scrollRect.verticalNormalizedPosition;

        while (Mathf.Abs(currentPos - target) > 0.001f)
        {
            currentPos = Mathf.Lerp(currentPos, target, Time.deltaTime * snapSpeed);

            if (scrollRect.horizontal)
                scrollRect.horizontalNormalizedPosition = currentPos;
            else
                scrollRect.verticalNormalizedPosition = currentPos;

            yield return null;
        }
        
        if (scrollRect.horizontal)
            scrollRect.horizontalNormalizedPosition = target;
        else
            scrollRect.verticalNormalizedPosition = target;
    }
}