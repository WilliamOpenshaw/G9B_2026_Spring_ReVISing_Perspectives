using UnityEngine;
using TMPro;

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

    [Header("Game End Screens")]
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI gameTimerText;
    public UnityEngine.UI.Image laundryTimerFillCircle;

    [Header("Game Timer Settings")]
    public float gameTimeLimit = 90f; // 1 min 30 seconds
    private float gameTimer;
    private bool playerWon = false;

    [Header("Laundry Counter")]
    public int laundryItemsNeeded = 50;
    private int laundryItemsCompleted = 0;

    // Game State Flags
    public bool isBabyCrying = false;
    public bool isGameActive = true;
    private bool isGameOver = false;

    void OnEnable()
    {
        // Reset all states and clear any active game-over/win states from yesterday
        isGameOver = false;
        playerWon = false;
        isBabyCrying = false;
        isCueActive = false;
        //laundryItemsCompleted = 0; 
        gameTimer = gameTimeLimit; 
        Time.timeScale = 1f; // Make sure the engine isn't frozen!

        // Hide win/lose panels
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (babyCryCueUI != null) babyCryCueUI.SetActive(false);

        // Reactivate timer for new game
        GameObject timer = GameObject.Find("timer");
        GameObject timerCircle = GameObject.Find("TimerCircle");
        if (timer != null) timer.SetActive(true);
        if (timerCircle != null) timerCircle.SetActive(true);

        GoToLaundryRoom();
        ResetCryTimer();
    }

    
    void Update()
    {
        if (isGameOver) return;

        // Count down game timer
        gameTimer -= Time.deltaTime;
        if (gameTimerText != null)
        {
            int minutes = Mathf.FloorToInt(gameTimer / 60);
            int seconds = Mathf.FloorToInt(gameTimer % 60);
            gameTimerText.text = $"<color=red>{minutes}:{seconds:D2}</color>";
        }

        // ⚪ NEW: Smoothly un-fill the circle along with the 90-second limit!
        if (laundryTimerFillCircle != null)
        {
            laundryTimerFillCircle.fillAmount = gameTimer / gameTimeLimit;
        }

        // Check if time ran out (player loses!)
        if (gameTimer <= 0)
        {
            playerWon = false;
            TriggerGameEnd();
            return;
        }

        // Baby can only cry when player is in the laundry room
        if (!isBabyCrying && laundryPanel.activeSelf) //
        {
            // CRITICAL SAFETY CHECK: Only count down the cry timer if the player is on HARD MODE!
            if (DifficultyManager.CurrentMode == DifficultyManager.GameMode.Hard)
            {
                cryEventTimer -= Time.deltaTime; //
                if (cryEventTimer <= 0) //
                {
                    TriggerCryAlert(); //
                }
            }
            else
            {
                // On Easy or Medium, make sure the alert UI can NEVER accidentally turn on
                if (babyCryCueUI != null && babyCryCueUI.activeSelf) 
                    babyCryCueUI.SetActive(false);
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

        // Only show timer if game is still active
        if (!isGameOver)
        {
            GameObject timer = GameObject.Find("timer");
            GameObject timerCircle = GameObject.Find("TimerCircle");
            if (timer != null) timer.SetActive(true);
            if (timerCircle != null) timerCircle.SetActive(true);
        }

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

    public void CompleteLaundryItem()
    {
        laundryItemsCompleted++;
        Debug.Log("Laundry completed: " + laundryItemsCompleted + "/" + laundryItemsNeeded);

        // Check if player won
        if (laundryItemsCompleted >= laundryItemsNeeded)
        {
            // Kill the timer immediately
            GameObject timer = GameObject.Find("timer");
            GameObject timerCircle = GameObject.Find("TimerCircle");
            if (timer != null) timer.SetActive(false);
            if (timerCircle != null) timerCircle.SetActive(false);

            if (winPanel != null) winPanel.SetActive(true);
            playerWon = true;
            isGameOver = true;
            TriggerGameEnd();
        }
    }

    public void CompleteBabyTameSuccess()
    {
        isBabyCrying = false; 
        isCueActive = false; // Clear the alert cue
        babyCryCueUI.SetActive(false); // Make sure it's hidden
        if (balanceGameScript != null) balanceGameScript.ShowEmptyRoom();
        // Don't kick them out - let them stay in the nursery and click a button to go to maze
        ResetCryTimer();
    }

    public void TriggerBabyCryGameOver()
    {
        if (isGameOver) return;
        playerWon = false; // Player lost
        TriggerGameEnd();
    }

    void TriggerGameEnd()
    {
        // Find timer objects by name and deactivate them
        GameObject timer = GameObject.Find("timer");
        GameObject timerCircle = GameObject.Find("TimerCircle");
        
        if (timer != null) 
        {
            timer.SetActive(false);
            Debug.Log("✓ Deactivated timer object");
        }
        
        if (timerCircle != null)
        {
            timerCircle.SetActive(false);
            Debug.Log("✓ Deactivated TimerCircle object");
        }

        // Set the state flag
        isGameOver = true;

        // 2. Hide all the room panels
        if (laundryPanel != null) laundryPanel.SetActive(false);
        if (livingRoomPanel != null) livingRoomPanel.SetActive(false);
        if (livingRoomBackPanel != null) livingRoomBackPanel.SetActive(false);
        if (nurseryPanel != null) nurseryPanel.SetActive(false);
        if (babyCryCueUI != null) babyCryCueUI.SetActive(false);

        // 3. Show appropriate end screen
        if (playerWon)
        {
            if (winPanel != null) winPanel.SetActive(true);
            Debug.Log("PLAYER WON!");
        }
        else
        {
            if (losePanel != null) losePanel.SetActive(true);
            Debug.LogError("PLAYER LOST!");
        }

        // 4. FREEZE TIME LAST
        Time.timeScale = 0f; 
    }
    void ResetCryTimer()
    {
        cryEventTimer = Random.Range(minTimeBetweenCries, maxTimeBetweenCries);
    }

    
    public void CompleteMazeToNursery() { GoToNurseryRoom(); }
    public void CompleteMazeToLaundry() { GoToLaundryRoom(); }
}