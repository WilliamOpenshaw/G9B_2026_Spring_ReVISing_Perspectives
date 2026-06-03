using UnityEngine;
using UnityEngine.EventSystems;

public class VacuumDraggable : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private CleaningGame gameManager;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        gameManager = Object.FindFirstObjectByType<CleaningGame>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // This makes the vacuum nozzle follow your mouse instantly!
        rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;

        // Every single millimeter we move, tell the manager to check if we ran over any dust!
        if (gameManager != null)
        {
            gameManager.CheckVacuumCollision(rectTransform);
        }
    }
}