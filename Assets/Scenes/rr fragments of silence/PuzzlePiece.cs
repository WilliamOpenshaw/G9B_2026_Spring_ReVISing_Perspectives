using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [Header("Target Alignment")]
    [SerializeField] private Transform guidelineTarget; 
    [SerializeField] private float snapThreshold = 40f; 

    private Vector3 offset;
    private bool isSnapped = false;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer targetRenderer;
    private int originalSortingOrder;

    public bool IsSnapped() => isSnapped;
    
    // PUBLIC ACCESS METHOD: Clears out error CS1061 by giving PuzzleManager full access!
    public SpriteRenderer GetGuidelineRenderer() => targetRenderer;

    void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null) mainCamera = FindFirstObjectByType<Camera>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) originalSortingOrder = spriteRenderer.sortingOrder;

        if (guidelineTarget != null)
        {
            targetRenderer = guidelineTarget.GetComponent<SpriteRenderer>();
            if (targetRenderer != null)
            {
                // FORCE THE TARGET INTO A SOLID SEE-THROUGH WHITE SILHOUETTE
                // Changing the material to an unlit GUI type eliminates artwork details/paper textures
                targetRenderer.material = new Material(Shader.Find("UI/Default"));
                targetRenderer.color = new Color(1f, 1f, 1f, 0.2f); // 20% alpha see-through template
            }
        }
    }

    void OnMouseDown()
    {
        if (isSnapped || mainCamera == null) return;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePos.x, mousePos.y, transform.position.z);
        if (spriteRenderer != null) spriteRenderer.sortingOrder = 100;
    }

    void OnMouseDrag()
    {
        if (isSnapped || mainCamera == null) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z) + offset;

        if (guidelineTarget != null && targetRenderer != null)
        {
            float distance = Vector2.Distance(transform.position, guidelineTarget.position);
            if (distance <= snapThreshold)
            {
                targetRenderer.color = new Color(1f, 1f, 1f, 0.5f); // Brightens up nicely as you hover over it
            }
            else
            {
                targetRenderer.color = new Color(1f, 1f, 1f, 0.2f); // Back to light hint
            }
        }
    }

    void OnMouseUp()
    {
        if (isSnapped || guidelineTarget == null || mainCamera == null) return;
        if (spriteRenderer != null) spriteRenderer.sortingOrder = originalSortingOrder;

        float distance = Vector2.Distance(transform.position, guidelineTarget.position);

        if (distance <= snapThreshold)
        {
            transform.position = guidelineTarget.position;
            transform.rotation = guidelineTarget.rotation;
            isSnapped = true;

            // Turn off the guideline temporary so it doesn't overlap the neat paper art
            if (targetRenderer != null) targetRenderer.enabled = false;

            FindFirstObjectByType<PuzzleManager>()?.CheckPuzzleCompletion();
        }
        else
        {
            if (targetRenderer != null) targetRenderer.color = new Color(1f, 1f, 1f, 0.2f);
        }
    }
}