using UnityEngine;
using System.Collections.Generic;

public class MazePlayerController : MonoBehaviour
{
    public float moveSpeed = 300f;       
    public RectTransform startPosition;  
    public RectTransform exitZone;    

    [Header("Obstacle Lists")]
    public List<RectTransform> solidWalls; 
    public List<RectTransform> teleportTraps; 

    private RectTransform rectTransform;
    private LaundryBabyManager gameManager;

    public bool isGoingToNursery = true; 

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        gameManager = Object.FindFirstObjectByType<LaundryBabyManager>();
        ResetToStart();
    }

    void Update()
    {
        if (gameManager != null && !gameManager.isGameActive) return;

        HandleMovementWithWalls();
        CheckTeleportTraps();
        CheckExitCollision();
    }

    void HandleMovementWithWalls()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical");   

        if (moveX == 0 && moveY == 0) return;

        Vector2 movement = new Vector2(moveX, moveY).normalized * moveSpeed * Time.deltaTime;
        
        // 1. Test Horizontal Step safely using absolute world space
        Vector3 originalWorldPos = rectTransform.position;
        rectTransform.anchoredPosition += new Vector2(movement.x, 0);
        
        if (WillCollideWithWalls(rectTransform))
        {
            rectTransform.position = originalWorldPos; // Revert if X hits a wall
        }

        // 2. Test Vertical Step safely using absolute world space
        originalWorldPos = rectTransform.position;
        rectTransform.anchoredPosition += new Vector2(0, movement.y);
        
        if (WillCollideWithWalls(rectTransform))
        {
            rectTransform.position = originalWorldPos; // Revert if Y hits a wall
        }
    }

    bool WillCollideWithWalls(RectTransform playerRect)
    {
        if (solidWalls == null || solidWalls.Count == 0) return false;

        foreach (RectTransform wall in solidWalls)
        {
            if (wall == null) continue;
            if (IsOverlappingInWorldSpace(playerRect, wall))
            {
                return true; 
            }
        }
        return false; 
    }

    void CheckTeleportTraps()
    {
        if (teleportTraps == null || teleportTraps.Count == 0) return;

        foreach (RectTransform trap in teleportTraps)
        {
            if (trap == null) continue;

            if (IsOverlappingInWorldSpace(rectTransform, trap))
            {
                ResetToStart();
                break;
            }
        }
    }

    void CheckExitCollision()
    {
        if (exitZone == null) return;

        if (IsOverlappingInWorldSpace(rectTransform, exitZone))
        {
            if (gameManager != null)
            {
                if (isGoingToNursery) gameManager.CompleteMazeToNursery();
                else gameManager.CompleteMazeToLaundry();
            }
        }
    }

    // NEW WORLD-SPACE COLLISION MATH: Uses screen space boundaries instead of local anchor points
    bool IsOverlappingInWorldSpace(RectTransform rectA, RectTransform rectB)
    {
        Vector3[] cornersA = new Vector3[4];
        Vector3[] cornersB = new Vector3[4];
        
        rectA.GetWorldCorners(cornersA);
        rectB.GetWorldCorners(cornersB);

        // corners: 0 = bottom-left, 1 = top-left, 2 = top-right, 3 = bottom-right
        bool xOverlap = (cornersA[0].x < cornersB[2].x) && (cornersA[2].x > cornersB[0].x);
        bool yOverlap = (cornersA[0].y < cornersB[2].y) && (cornersA[2].y > cornersB[0].y);

        return xOverlap && yOverlap;
    }

    public void ResetToStart()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        if (startPosition != null && rectTransform != null)
        {
            rectTransform.position = startPosition.position;
        }
    }
}