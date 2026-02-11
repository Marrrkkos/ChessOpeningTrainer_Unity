using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Text fieldName;
    public Image currentPieceImage;
    public Board board;

    public void Awake() {
        board = GetComponentInParent<Board>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        board.actionService.setFieldOnMouseDown(fieldName.text);
        //Debug.Log(fieldName.text);
        //Debug.Log(GetComponent<Field>().piece.ToString() + " " +  GetComponent<Field>().piece.color.ToString());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject hoveredObject = eventData.pointerCurrentRaycast.gameObject;

        if (hoveredObject != null)
        {
            ClickHandler targetField = hoveredObject.GetComponentInParent<ClickHandler>();

            if (targetField != null)
            {
                board.actionService.setFieldOnMouseUp(targetField.fieldName.text);
                return;
            }
        }

    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnDrag(PointerEventData eventData) { 
    }
}
