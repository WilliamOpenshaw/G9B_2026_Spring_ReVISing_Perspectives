using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MagnifyingGlass : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [Header("Reveal Framework References")]
    [SerializeField] private RectTransform sharpLetterRect;
    [SerializeField] private RectTransform clueTargetArea;
    [SerializeField] private Image dirtCoverOverlay;
    [SerializeField] private float completionHoldTime = 1.5f;
    
    [Header("Upgraded Scrubbing Settings")]
    [SerializeField] private RenderTexture maskRenderTexture;
    [SerializeField] private Texture2D brushSprite;
    [SerializeField] private float brushSize = 1.2f; // Increased default size for HD canvases
    [Range(0f, 1f)] [SerializeField] private float targetCleanPercentage = 0.75f;

    [Header("Screen Swapping Navigation")]
    [SerializeField] private GameObject investigationWorkbenchScreen;
    [SerializeField] private GameObject gameplayChoiceScreen;

    private RectTransform lensRect;
    private Canvas parentCanvas;
    private float currentHoverTime = 0f;
    private bool isDirtCleaned = false;
    private bool isTaskComplete = false;

    // Pixel tracking variables
    private Texture2D checkTexture;
    private int totalPixels;
    private int clearedPixels = 0;
    private float checkTimer = 0f;

    // Trail tracking to connect mouse steps
    private Vector2? lastPosition = null;

    void Awake()
    {
        lensRect = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();

        // Wipe the texture to solid black at start
        RenderTexture.active = maskRenderTexture;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = null;

        checkTexture = new Texture2D(maskRenderTexture.width, maskRenderTexture.height, TextureFormat.RGB24, false);
        totalPixels = checkTexture.width * checkTexture.height;

        gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isTaskComplete) return;
        
        if (isDirtCleaned)
        {
            MoveLens(eventData);
        }
        else
        {
            // Start a brand new stroke line where the player clicked
            lastPosition = eventData.position;
            PaintMaskElement(eventData.position);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void CleanDirtWithMouse(BaseEventData eventData)
    {
        if (isTaskComplete) return;

        PointerEventData pointerData = eventData as PointerEventData;
        if (pointerData == null) return;

        if (!isDirtCleaned)
        {
            Vector2 currentPosition = pointerData.position;

            if (lastPosition.HasValue)
            {
                // Measure the screen distance between this frame and the last frame
                float distance = Vector2.Distance(lastPosition.Value, currentPosition);
                
                // If the mouse skipped ahead, interpolate stamps along the line to bridge the gap
                if (distance > 2f)
                {
                    int steps = Mathf.CeilToInt(distance / 5f); // Step every 5 pixels
                    for (int i = 0; i <= steps; i++)
                    {
                        float t = (float)i / steps;
                        Vector2 lerpedPoint = Vector2.Lerp(lastPosition.Value, currentPosition, t);
                        PaintMaskElement(lerpedPoint);
                    }
                }
                else
                {
                    PaintMaskElement(currentPosition);
                }
            }
            else
            {
                PaintMaskElement(currentPosition);
            }

            lastPosition = currentPosition;
            
            checkTimer += Time.deltaTime;
            if (checkTimer >= 0.1f)
            {
                checkTimer = 0f;
                CalculateDirtCleanedPercentage();
            }
            return;
        }

        if (isDirtCleaned)
        {
            MoveLens(pointerData);
            CheckIfClueFound();
        }
    }

    // Reset stroke when lifting the pointer up
    public void ResetStroke()
    {
        lastPosition = null;
    }

    private void PaintMaskElement(Vector2 screenPos)
    {
        RectTransform overlayRect = dirtCoverOverlay.rectTransform;
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(overlayRect, screenPos, parentCanvas.worldCamera, out Vector2 localPoint))
        {
            float x = (localPoint.x - overlayRect.rect.x) / overlayRect.rect.width;
            float y = (localPoint.y - overlayRect.rect.y) / overlayRect.rect.height;

            RenderTexture.active = maskRenderTexture;
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, maskRenderTexture.width, 0, maskRenderTexture.height);

            Graphics.DrawTexture(
                new Rect(x * maskRenderTexture.width - (brushSize * 50), y * maskRenderTexture.height - (brushSize * 50), brushSize * 100, brushSize * 100), 
                brushSprite != null ? brushSprite : Texture2D.whiteTexture
            );

            GL.PopMatrix();
            RenderTexture.active = null;
        }
    }

    private void CalculateDirtCleanedPercentage()
    {
        RenderTexture.active = maskRenderTexture;
        checkTexture.ReadPixels(new Rect(0, 0, maskRenderTexture.width, maskRenderTexture.height), 0, 0);
        RenderTexture.active = null;

        Color[] pixels = checkTexture.GetPixels();
        clearedPixels = 0;

        for (int i = 0; i < pixels.Length; i += 10) 
        {
            if (pixels[i].r > 0.5f)
            {
                clearedPixels += 10;
            }
        }

        float currentProgress = (float)clearedPixels / totalPixels;

        if (currentProgress >= targetCleanPercentage)
        {
            isDirtCleaned = true;
            dirtCoverOverlay.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    private void MoveLens(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform, 
            eventData.position, 
            parentCanvas.worldCamera, 
            out Vector2 localPoint))
        {
            lensRect.anchoredPosition = localPoint;

            if (sharpLetterRect != null)
            {
                sharpLetterRect.anchoredPosition = -localPoint;
            }
        }
    }

    private void CheckIfClueFound()
    {
        if (clueTargetArea == null || isTaskComplete) return;

        float distance = Vector2.Distance(lensRect.anchoredPosition, clueTargetArea.anchoredPosition);

        if (distance <= 60f)
        {
            currentHoverTime += Time.deltaTime;

            if (currentHoverTime >= completionHoldTime)
            {
                CompleteTask();
            }
        }
        else
        {
            currentHoverTime = 0f;
        }
    }

    private void CompleteTask()
    {
        isTaskComplete = true;
        Debug.Log("Clue Discovered!");
        if (investigationWorkbenchScreen != null) investigationWorkbenchScreen.SetActive(false);
        if (gameplayChoiceScreen != null) gameplayChoiceScreen.SetActive(true);
    }
}