using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LaundrySortingGame : MonoBehaviour
{
    [Header("UI Panels & Text")]
    public GameObject laundryPanel;       
    public Image clothingDisplayItem;    
    public TextMeshProUGUI progressText; 

    [Header("Game Settings")]
    public List<Color> coloredPool;       
    public int itemsToWin = 50;           

    [Header("Juice & FX")]
    public GameObject minusOnePopup; // Drag your MinusOneText object here!

    private string currentColorType = "White"; 
    private int currentSortedCount = 0;
    private bool isGameActive = false;
    private LaundryBabyManager gameManager;

    void Awake()
    {
        gameManager = Object.FindFirstObjectByType<LaundryBabyManager>();
    }

    void Start()
    {
        InitializeLaundryGame();
        StartSortingGame(); // Kept on for testing!
    }

    public void InitializeLaundryGame()
    {
        currentSortedCount = 0;
        UpdateScoreUI();
    }

    public void StartSortingGame()
    {
        laundryPanel.SetActive(true);
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
        if (!isGameActive) return;

        // A KEY: Must be White
        if (Input.GetKeyDown(KeyCode.A))
        {
            CheckPlayerChoice("White");
        }
        // R KEY: Must be Red
        else if (Input.GetKeyDown(KeyCode.R))
        {
            CheckPlayerChoice("Red");
        }
        // D KEY: Must be Blue
        else if (Input.GetKeyDown(KeyCode.D))
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
    }
}