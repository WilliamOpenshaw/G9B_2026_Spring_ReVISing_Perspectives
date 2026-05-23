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
        // Don't call ResetToStart() here - startPosition won't be set until LaundryBabyManager configures it
    }

    void Update()
    {
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
        
        Vector2 originalPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition += movement;

        if (WillCollideWithWalls())
        {
            rectTransform.anchoredPosition = originalPos; 
        }
    }

    bool WillCollideWithWalls()
    {
        if (solidWalls == null || solidWalls.Count == 0) return false;

        Vector3[] playerCorners = new Vector3[4];
        rectTransform.GetWorldCorners(playerCorners);

        foreach (RectTransform wall in solidWalls)
        {
            if (wall == null) continue;

            foreach (Vector3 corner in playerCorners)
            {
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);
                
                if (RectTransformUtility.RectangleContainsScreenPoint(wall, screenPoint, null))
                {
                    return true; 
                }
            }
        }
        return false; 
    }

    void CheckTeleportTraps()
    {
        if (teleportTraps == null || teleportTraps.Count == 0) return;

        Vector3[] playerCorners = new Vector3[4];
        rectTransform.GetWorldCorners(playerCorners);

        foreach (RectTransform trap in teleportTraps)
        {
            if (trap == null) continue;

            foreach (Vector3 corner in playerCorners)
            {
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);
                if (RectTransformUtility.RectangleContainsScreenPoint(trap, screenPoint, null))
                {
                    ResetToStart();
                    return;
                }
            }
        }
    }

    void CheckExitCollision()
    {
        if (exitZone == null) return;

        Vector3[] playerCorners = new Vector3[4];
        rectTransform.GetWorldCorners(playerCorners);

        foreach (Vector3 corner in playerCorners)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);
            if (RectTransformUtility.RectangleContainsScreenPoint(exitZone, screenPoint, null))
            {
                if (gameManager != null)
                {
                    if (isGoingToNursery) gameManager.GoToNurseryRoom();
                    else gameManager.GoToLaundryRoom();
                }
                return;
            }
        }
    }

    public void ResetToStart()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        if (startPosition != null && rectTransform != null)
        {
            rectTransform.position = startPosition.position;
        }
    }

    // Call this to turn on the maze and reset the player position
    public void StartMazeGame()
    {
        gameObject.SetActive(true);
        ResetToStart(); // Make sure this function teleports rectTransform to startPosition!
    }

    // Call this to completely hide the maze layout
    public void HideMazeGame()
    {
        gameObject.SetActive(false); // Disappears the maze entirely
    }
}