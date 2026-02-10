using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class SwipeController : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public float snapSpeed = 10f;

    private int totalPages;
    private float[] pagePositions;
    private bool isSnapping = false;

    void Start()
    {
        UpdatePages();
    }

    // Falls du während des Spiels Seiten hinzufügst, ruf diese Methode auf
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
        // Stop das Snapping, wenn der User wieder anfängt zu ziehen
        StopAllCoroutines();
        isSnapping = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float currentPos = scrollRect.horizontalNormalizedPosition;
        float nearest = float.MaxValue;
        int nearestPage = 0;

        // Finde die nächste Seite
        for (int i = 0; i < totalPages; i++)
        {
            float dist = Mathf.Abs(currentPos - pagePositions[i]);
            if (dist < nearest)
            {
                nearest = dist;
                nearestPage = i;
            }
        }

        StartCoroutine(SmoothSnap(pagePositions[nearestPage]));
    }

    private IEnumerator SmoothSnap(float target)
    {
        isSnapping = true;
        while (Mathf.Abs(scrollRect.horizontalNormalizedPosition - target) > 0.0001f)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(
                scrollRect.horizontalNormalizedPosition,
                target,
                Time.deltaTime * snapSpeed
            );
            yield return null;
        }
        scrollRect.horizontalNormalizedPosition = target;
        isSnapping = false;
    }
}