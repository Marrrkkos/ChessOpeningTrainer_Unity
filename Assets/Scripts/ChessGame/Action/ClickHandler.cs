using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public string fieldName;
    public Image currentPieceImage;
    public Image onPossibleHoverImage;
    public Image lastMoveImage;
    public Image selectedPieceImage;
    public Board board;

    private Image ghost;
    private Color ghostColor;
    private Color normalColor;
    public void Awake() {
        board = GetComponentInParent<Board>();
        ghostColor = currentPieceImage.color;
        normalColor = currentPieceImage.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("test");
        board.fields[board.actionService.selectedField].selectedPieceImage.gameObject.SetActive(false);
        Debug.Log(fieldName);
        board.actionService.SetFieldOnMouseDown(fieldName);
        //Debug.Log(GetComponent<Field>().piece.ToString() + " " +  GetComponent<Field>().piece.color.ToString());

        if (board.actionService.CheckOwnPiece(fieldName))
        {   
            int id = BoardUtil.StringToIndex(fieldName);
            board.actionService.SetSelectedField(id);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject hoveredObject = eventData.pointerCurrentRaycast.gameObject;

        if (hoveredObject != null)
        {
            ClickHandler targetField = hoveredObject.GetComponentInParent<ClickHandler>();

            if (targetField != null)
            {
                Debug.Log(targetField.fieldName);
                board.actionService.SetFieldOnMouseUp(targetField.fieldName);
                return;
            }
        }

    }
    public void OnPointerEnter(PointerEventData eventData)
{
    Debug.Log("Hover über: " + fieldName);

    if (board.actionService.CheckPossibleField(fieldName))
    {
        onPossibleHoverImage.gameObject.SetActive(true);
    }
}

public void OnPointerExit(PointerEventData eventData)
{
    onPossibleHoverImage.gameObject.SetActive(false);
}
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        ghost = Instantiate(currentPieceImage, board.fields[63].transform); // LAST FIELD SO ITS ABOVE ALL

        currentPieceImage.color = ghostColor;
        ghost.transform.localScale = Vector3.one * 1.2f;

        ghost.transform.position = currentPieceImage.transform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Canvas canvas = currentPieceImage.canvas;
        ghost.rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
        currentPieceImage.color = normalColor;
        Destroy(ghost.gameObject);
    }
}
