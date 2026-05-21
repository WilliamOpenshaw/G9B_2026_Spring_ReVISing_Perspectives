using UnityEngine;
using UnityEngine.EventSystems;

public class TrashDraggable : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private CleaningGame gameManager;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        // Find the main canvas in your scene automatically
        parentCanvas = GetComponentInParent<Canvas>();
        // Find our main game script
        gameManager = Object.FindFirstObjectByType<CleaningGame>();
    }

    // This forces the trash item to follow the mouse instantly and perfectly!
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
    }

    // This fires the exact moment the player lets go of the mouse click
    public void OnEndDrag(PointerEventData eventData)
    {
        // Ask the manager if we were dropped near the trash can
        if (gameManager != null)
        {
            gameManager.CheckTrashDrop(gameObject);
        }
    }
}