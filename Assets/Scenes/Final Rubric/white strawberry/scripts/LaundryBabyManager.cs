using UnityEngine;

public class LaundryBabyManager : MonoBehaviour
{
    [Header("The 4 Main Room Panels")]    
    public GameObject laundryPanel;
    public GameObject livingRoomPanel;     // Maze 1: Laundry -> Nursery
    public GameObject livingRoomBackPanel; // Maze 2: Nursery -> Laundry
    public GameObject nurseryPanel;

    [Header("Alert Cue (Laundry Room)")]
    public GameObject babyCryCueUI; 
    public float responseTimeLimit = 15f; 
    private float responseTimer;
    private bool isCueActive = false;

    [Header("Random Cry Event Settings")]
    public float minTimeBetweenCries = 10f; 
    public float maxTimeBetweenCries = 25f; 
    private float cryEventTimer;

    [Header("Script References")]
    public BabyBalanceGame balanceGameScript; 
    public MazePlayerController mazeScript; 

    [Header("Maze 1 Configuration")]
    public RectTransform maze1Start;
    public RectTransform maze1Exit;

    [Header("Maze 2 Configuration")]
    public RectTransform maze2Start;
    public RectTransform maze2Exit;

    // Game State Flags
    public bool isBabyCrying = false;
    public bool isGameActive = true; 
    private bool isGameOver = false;

    void Start()
    {
        GoToLaundryRoom();
        ResetCryTimer();
    }

    void Update()
    {
        if (isGameOver) return;

        if (!isBabyCrying)
        {
            cryEventTimer -= Time.deltaTime;
            if (cryEventTimer <= 0)
            {
                TriggerCryAlert();
            }
        }

        if (isCueActive && !nurseryPanel.activeSelf)
        {
            responseTimer -= Time.deltaTime;
            if (responseTimer <= 0)
            {
                TriggerBabyCryGameOver(); 
            }
        }
    }

    void TriggerCryAlert()
    {
        isBabyCrying = true;
        isCueActive = true;
        babyCryCueUI.SetActive(true);
        responseTimer = responseTimeLimit;
        Debug.Log("THE BABY IS CRYING!");
    }

    // ==========================================
    // ROOM NAVIGATION
    // ==========================================

    public void GoToLaundryRoom()
    {
        laundryPanel.SetActive(true);
        livingRoomPanel.SetActive(false);
        livingRoomBackPanel.SetActive(false);
        nurseryPanel.SetActive(false);
        
        if (balanceGameScript != null) balanceGameScript.ShowEmptyRoom();
        babyCryCueUI.SetActive(isCueActive); 

        if (mazeScript != null) mazeScript.gameObject.SetActive(false);
    }

    // Entering Maze 1 (Laundry -> Nursery)
    public void GoToLivingRoomForward()
    {
        laundryPanel.SetActive(false);
        livingRoomPanel.SetActive(true);
        livingRoomBackPanel.SetActive(false);
        nurseryPanel.SetActive(false);

        if (balanceGameScript != null) balanceGameScript.ShowEmptyRoom();
        babyCryCueUI.SetActive(false); 

        if (mazeScript != null) 
        {
            // Update variables safely without touching the parent hierarchy
            mazeScript.startPosition = maze1Start;
            mazeScript.exitZone = maze1Exit;
            mazeScript.isGoingToNursery = true;

            mazeScript.StartMazeGame(); 
        }
    }

    // Entering Maze 2 (Nursery -> Laundry)
    public void GoToLivingRoomBackward()
    {
        laundryPanel.SetActive(false);
        livingRoomPanel.SetActive(false);
        livingRoomBackPanel.SetActive(true);
        nurseryPanel.SetActive(false);

        if (balanceGameScript != null) balanceGameScript.ShowEmptyRoom();
        babyCryCueUI.SetActive(false); 

        if (mazeScript != null) 
        {
            // Update variables safely without touching the parent hierarchy
            mazeScript.startPosition = maze2Start;
            mazeScript.exitZone = maze2Exit;
            mazeScript.isGoingToNursery = false;

            mazeScript.StartMazeGame(); 
        }
    }

    public void GoToLivingRoomMaze() { GoToLivingRoomForward(); }

    public void GoToNurseryRoom()
    {
        laundryPanel.SetActive(false);
        livingRoomPanel.SetActive(false);
        livingRoomBackPanel.SetActive(false);
        nurseryPanel.SetActive(true);

        isCueActive = false;
        babyCryCueUI.SetActive(false);

        if (mazeScript != null) mazeScript.gameObject.SetActive(false);

        if (isBabyCrying)
        {
            if (balanceGameScript != null) balanceGameScript.StartTamingGame();
        }
        else
        {
            if (balanceGameScript != null) balanceGameScript.ShowEmptyRoom();
        }
    }

    // ==========================================
    // GAME LOGIC
    // ==========================================

    public void CompleteBabyTameSuccess()
    {
        isBabyCrying = false; 
        if (balanceGameScript != null) balanceGameScript.ShowEmptyRoom();

        GoToLivingRoomBackward(); 
        ResetCryTimer();
    }

    public void TriggerBabyCryGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.LogError("GAME OVER!");
        Time.timeScale = 0f; 
    }

    void ResetCryTimer()
    {
        cryEventTimer = Random.Range(minTimeBetweenCries, maxTimeBetweenCries);
    }

    public void CompleteMazeToNursery() { GoToNurseryRoom(); }
    public void CompleteMazeToLaundry() { GoToLaundryRoom(); }
}