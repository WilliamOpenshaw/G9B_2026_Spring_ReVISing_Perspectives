using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LaundrySortingGame : MonoBehaviour
{
    [Header("UI Panels & Text")]
    public GameObject laundryPanel;
    public GameObject winPanel;       
    public Image clothingDisplayItem;    
    public TextMeshProUGUI progressText; 
    public GameObject goToLivingRoomButton;

    [Header("Game Settings")]
    public List<Color> coloredPool;       
    public int itemsToWin = 50;           

    [Header("Juice & FX")]
    public GameObject minusOnePopup; // Drag your MinusOneText object here!

    private string currentColorType = "White"; 
    private int currentSortedCount = 0;
    private bool isGameActive = false;
    public LaundryBabyManager gameManager;

    void Awake()
    {
        //gameManager = Object.FindFirstObjectByType<LaundryBabyManager>();
    }

    void OnEnable()
    {
        // 1. FORCE THE INTERNAL COUNTER BACK TO ZERO!
        // (Make sure this variable name matches whatever integer counts your clothes, 
        // like 'sortedClothesCount', 'score', or 'itemsSorted')
        currentSortedCount = 0; 

        isGameActive = true; // Enables player inputs

        // 2. DYNAMIC DIFFICULTY ADJUSTMENT: Set how many clothes are needed to win
        if (DifficultyManager.CurrentMode == DifficultyManager.GameMode.Baby) // Easy Mode
        {
            itemsToWin = 20; 
        }
        else if (DifficultyManager.CurrentMode == DifficultyManager.GameMode.Easy) // Medium Mode
        {
            itemsToWin = 30;
        }
        else // Hard Mode
        {
            itemsToWin = 50;
        }

        // Set your panels safely
        if (laundryPanel != null) laundryPanel.SetActive(true); //
        if (winPanel != null) winPanel.SetActive(false);        //

        // 3. FORCE THE SCREEN TEXT TO SHOW THE 0 IMMEDIATELY!
        UpdateScoreUI();     // Refreshes text display with new targets
        SpawnNextClothing(); // Spawns first clothing item
        
        Debug.Log($"Laundry Game Woke Up! Counter forced to: {currentSortedCount} / {itemsToWin}");
    }
    public void InitializeLaundryGame()
    {
        currentSortedCount = 0;
        UpdateScoreUI();

    }

    public void StartSortingGame()
    {
        laundryPanel.SetActive(true);
        currentSortedCount = 0; 
        isGameActive = true;
        UpdateScoreUI();
        SpawnNextClothing();
    }

    public void PauseAndLeaveLaundry()
    {
        isGameActive = false;
        laundryPanel.SetActive(false);
    }



    

    void Update()
    {
        // --- BABY CRYING SAFETY LOCK ---
        if (goToLivingRoomButton != null && gameManager != null)
        {
            // Read the true/false crying value from your master manager
            bool cryingState = gameManager.isBabyCrying; 

            // Turn the ENTIRE button GameObject active when crying, and inactive when quiet!
            if (goToLivingRoomButton.activeSelf != cryingState)
            {
                goToLivingRoomButton.SetActive(cryingState);
            }
        }
        // -------------------------------

        if (!isGameActive) return;

        // A KEY: Must be White
        if (Input.GetKeyDown(KeyCode.A))
        {
            CheckPlayerChoice("White");
        }
        // F KEY: Middle Washing Machine (Red)
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CheckPlayerChoice("Red");
        }
        // J KEY: Right Washing Machine (Blue)
        else if (Input.GetKeyDown(KeyCode.J))
        {
            CheckPlayerChoice("Blue");
        }
    }

    void SpawnNextClothing()
    {
        int randomChoice = Random.Range(0, 3);

        if (randomChoice == 0)
        {
            clothingDisplayItem.color = Color.white;
            currentColorType = "White";
        }
        else if (randomChoice == 1)
        {
            if (coloredPool != null && coloredPool.Count > 0) clothingDisplayItem.color = coloredPool[0];
            currentColorType = "Red";
        }
        else if (randomChoice == 2)
        {
            if (coloredPool != null && coloredPool.Count > 1) clothingDisplayItem.color = coloredPool[1];
            currentColorType = "Blue";
        }
    }

    // This is the clean, combined version with the -1 popup effect inside!
    void CheckPlayerChoice(string playerPickedColor)
    {
        if (playerPickedColor == currentColorType)
        {
            // Correct sort! +1 point
            currentSortedCount++;
            UpdateScoreUI();

            if (currentSortedCount >= itemsToWin)
            {
                WinSortingGame();
                return;
            }
        }
        else
        {
            // Penalty for hitting the wrong key!
            Debug.Log("Wrong basket! -1 Point penalty!");
            currentSortedCount = Mathf.Max(0, currentSortedCount - 1); 
            UpdateScoreUI();

            // VISUAL POPUP TRIGGER:
            if (minusOnePopup != null)
            {
                // Cancel any previous hide timers so they don't overlap if you mash keys
                CancelInvoke("HideMinusOne"); 

                minusOnePopup.SetActive(true);  // Show the "-1 point" text

                // Tell Unity to run the "HideMinusOne" function automatically in 0.5 seconds!
                Invoke("HideMinusOne", 0.5f);   
            }
        }

        // Always switch the clothing item immediately so they can't get stuck
        SpawnNextClothing();
    }

    // This tiny function turns the popup off!
    void HideMinusOne()
    {
        if (minusOnePopup != null)
        {
            minusOnePopup.SetActive(false);
        }
    }

    void UpdateScoreUI()
    {
        if (progressText != null)
        {
            progressText.text = "Sorted: " + currentSortedCount + " / " + itemsToWin;
        }
    }

    void WinSortingGame()
    {
        isGameActive = false;
        laundryPanel.SetActive(false);
        Debug.Log("50 clothes sorted perfectly with zero exploits!");
        winPanel.SetActive(true);

    }

    // ==========================================
    // DAY RESET LOGIC: Call this to reset for a new day!
    // ==========================================
    public void ResetMiniGameForNewDay()
    {
        currentSortedCount = 0; // Wipe out the 50 clothes sorted yesterday
        isGameActive = false;   // Keep it quiet until the player enters the room

        // Make sure the gameplay screen is ready and win panel is turned off
        if (laundryPanel != null) laundryPanel.SetActive(false); 
        if (winPanel != null) winPanel.SetActive(false);

        UpdateScoreUI();
        Debug.Log("Laundry Sorting Game has been fully reset for the new day!");
    }

    public void ResetLaundryProgressForNewDay()
    {
        currentSortedCount = 0;
        
        // NEW: Also reach into the baby manager and wipe its tracking variable!
        if (gameManager != null)
        {
            // We use a clean assignment to force it back to zero
            System.Reflection.FieldInfo field = gameManager.GetType().GetField("laundryItemsCompleted", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (field != null) field.SetValue(gameManager, 0);
        }
        
        Debug.Log("Laundry score and manager score completely wiped for the start of a new day!");
    }
}