using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleClickerHandler : MonoBehaviour, IPointerDownHandler
{
    public string fieldName;

    private GameSenseTrainingController gameSenseTrainingController;


    public void Awake()
    {
        gameSenseTrainingController = GetComponentInParent<GameSenseTrainingController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameSenseTrainingController.LakChuch(fieldName);
    }
}
