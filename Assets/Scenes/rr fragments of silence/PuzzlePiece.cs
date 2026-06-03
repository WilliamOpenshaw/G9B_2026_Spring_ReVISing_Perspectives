using UnityEngine;
using UnityEngine.UI; // Required for Image components
using UnityEngine.EventSystems; // Required for UI Drag & Drop interfaces

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Target Alignment")]
    [SerializeField] private RectTransform guidelineTarget; // Swapped to RectTransform
    [SerializeField] private float snapThreshold = 40f; 

    private bool isSnapped = false;
    private Image imageComponent; // Swapped from SpriteRenderer
    private Image targetImageComponent; // Swapped from SpriteRenderer
    private int originalSiblingIndex; // Used instead of sortingOrder for UI layers
    
    private RectTransform rectTransform;
    private Canvas canvas;

    public bool IsSnapped() => isSnapped;
    
    // Public access method matching your manager's needs
    public Image GetGuidelineRenderer() => targetImageComponent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        imageComponent = GetComponent<Image>();
        
        if (imageComponent != null) originalSiblingIndex = transform.GetSiblingIndex();

        if (guidelineTarget != null)
        {
            targetImageComponent = guidelineTarget.GetComponent<Image>();
            if (targetImageComponent != null)
            {
                // UI Images use UI/Default by default, setting up the silhouette template
                targetImageComponent.material = new Material(Shader.Find("UI/Default"));
                targetImageComponent.color = new Color(1f, 1f, 1f, 0.2f); // 20% alpha template
            }
        }
    }

    // Triggers when the user first clicks/touches the UI piece
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapped) return;
        
        // Brings this UI element to the front of its UI layer hierarchy
        transform.SetAsLastSibling();
    }

    // Triggers continuously while dragging the UI piece
    public void OnDrag(PointerEventData eventData)
    {
        if (isSnapped || canvas == null) return;

        // Moves the piece smoothly by adjusting for canvas scaling automatically
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        if (guidelineTarget != null && targetImageComponent != null)
        {
            float distance = Vector2.Distance(rectTransform.anchoredPosition, guidelineTarget.anchoredPosition);
            if (distance <= snapThreshold)
            {
                targetImageComponent.color = new Color(1f, 1f, 1f, 0.5f); // Brighten on hover
            }
            else
            {
                targetImageComponent.color = new Color(1f, 1f, 1f, 0.2f); // Dim back down
            }
        }
    }

    // Triggers when the user releases the UI piece
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSnapped || guidelineTarget == null) return;
        
        // Restore its original depth sorting order inside the UI hierarchy
        transform.SetSiblingIndex(originalSiblingIndex);

        float distance = Vector2.Distance(rectTransform.anchoredPosition, guidelineTarget.anchoredPosition);

        if (distance <= snapThreshold)
        {
            // Snap perfectly to the target's position and rotation
            rectTransform.anchoredPosition = guidelineTarget.anchoredPosition;
            rectTransform.localRotation = guidelineTarget.localRotation;
            isSnapped = true;

            if (targetImageComponent != null) targetImageComponent.enabled = false;

            FindFirstObjectByType<PuzzleManager>()?.CheckPuzzleCompletion();
        }
        else
        {
            if (targetImageComponent != null) targetImageComponent.color = new Color(1f, 1f, 1f, 0.2f);
        }
    }
}