using UnityEngine;
using TMPro;

public class DayLoopManager : MonoBehaviour
{
    [Header("Core Calendar Stats")]
    public int currentDay = 1;
    public int targetDebtGoal = 250;

    [Header("Day Panel Objects")]
    public GameObject day1GameplayParent; // Drag your Day 1 stuff here
    public GameObject day2GameplayParent; // Drag your Day 2 stuff here
    public GameObject day3GameplayParent; // Drag your Day 3 stuff here

    [Header("YOUR Existing Script References")]
    public healthbar realHealthScript; 
    public moneymanager realMoneyScript; // Linked straight to your script now!

    [Header("End of Day Panel UI Slots")]
    public GameObject endOfDaySummaryPanel; 
    public TextMeshProUGUI summaryMessageText; 
    public TextMeshProUGUI sleepButtonLabelText; 

    [Header("End Game Canvas Panels")]
    public GameObject finalGameWinPanel;  // Drag your master Win Canvas Panel here
    public GameObject finalGameLosePanel; // Drag your master Game Over/Lose Canvas Panel here

    public EndGameBranchManager branchManagerScript;

    private bool canSleepAndHeal = true;

    // Call this if they pass a task
    public void ShowSummaryWithRest()
    {
        canSleepAndHeal = true;
        DisplayThePanel();
    }

    // Call this if they fail or get scammed
    public void ShowSummaryNoRest()
    {
        canSleepAndHeal = false;
        DisplayThePanel();
    }

    private void DisplayThePanel()
    {
        float displayHealth = 100f;
        int displayMoney = 0; 

        // 1. STEAL THE REAL HEALTH DIRECTLY FROM YOUR SLIDER VALUE!
        if (realHealthScript != null && realHealthScript.slider != null)
        {
            displayHealth = realHealthScript.slider.value; 
        }

        // 2. STEAL THE REAL MONEY DIRECTLY FROM YOUR MONEY MANAGER!
        if (realMoneyScript != null)
        {
            displayMoney = realMoneyScript.currentMoney; 
        }

        // 3. TURN ON THE POPUP PANEL
        if (endOfDaySummaryPanel != null) endOfDaySummaryPanel.SetActive(true);

        // 4. PRINT THE TRUTH TO THE TEXT BOX!
        if (canSleepAndHeal)
        {
            summaryMessageText.text = "Day " + currentDay + " Summary\n\n" +
                                     "Money: $" + displayMoney + " / $" + targetDebtGoal + "\n" +
                                     "Health: " + displayHealth + "%\n\n" +
                                     "You earned a decent night's rest.";
            sleepButtonLabelText.text = "Go to Sleep";
        }
        else
        {
            summaryMessageText.text = "Day " + currentDay + " Summary\n\n" +
                                     "Money: $" + displayMoney + " / $" + targetDebtGoal + "\n" +
                                     "Health: " + displayHealth + "%\n\n" +
                                     "You are too stressed and anxious to sleep tonight...";
            sleepButtonLabelText.text = "Stay Awake";
        }
    }

    public void ExecuteSleepButtonAction()
    {
        // (Keep your health restoration code here)
        if (canSleepAndHeal && realHealthScript != null && realHealthScript.slider != null)
        {
            int currentHP = (int)realHealthScript.slider.value;
            int newHP = currentHP + 10;
            if (newHP > 100) newHP = 100;
            realHealthScript.SetHealth(newHP); 
        }

        if (endOfDaySummaryPanel != null) endOfDaySummaryPanel.SetActive(false);

        LaundrySortingGame laundryScript = FindFirstObjectByType<LaundrySortingGame>();
        if (laundryScript != null)
        {
            laundryScript.ResetLaundryProgressForNewDay();
        }

        // Advance to the next day
        currentDay++;

        // DYNAMIC ROUTING: Turning on these parents will now automatically trigger the OnEnable() resets above!
        if (currentDay == 2)
        {
            if (day1GameplayParent != null) day1GameplayParent.SetActive(false); 
            if (day2GameplayParent != null) day2GameplayParent.SetActive(true);  
            Debug.Log("Welcome to Day 2!");
        }
        else if (currentDay == 3)
        {
            if (day2GameplayParent != null) day2GameplayParent.SetActive(false); 
            if (day3GameplayParent != null) day3GameplayParent.SetActive(true);  
            Debug.Log("Welcome to Day 3!");
        }
        else if (currentDay > 3)
        {
            Time.timeScale = 0f; // Freeze updates
            if (day3GameplayParent != null) day3GameplayParent.SetActive(false); 

            // NEW: Forcefully hide the HUD elements for ALL endings right here!
            if (branchManagerScript != null)
            {
                if (branchManagerScript.healthBarHUD != null) branchManagerScript.healthBarHUD.SetActive(false);
                if (branchManagerScript.moneyBarHUD != null) branchManagerScript.moneyBarHUD.SetActive(false);
            }

            // Get current health values from your active health slider script
            int playerFinalHealth = (realHealthScript != null && realHealthScript.slider != null) 
                ? (int)realHealthScript.slider.value 
                : 100;

            // ========================================================
            // HEALTH CRASH TRAP SYSTEM (Health is less than 20)
            // ========================================================
            if (playerFinalHealth < 20)
            {
                Debug.Log("Player health collapsed below 20! Booting into Crossroads.");
                if (branchManagerScript != null)
                {
                    branchManagerScript.LaunchCrossroadsGamble();
                }
                return; // HALT! Bypasses normal money calculations entirely
            }

            // ========================================================
            // STANDARD ENDINGS (Survives healthy)
            // ========================================================
            int finalMoney = (realMoneyScript != null) ? realMoneyScript.currentMoney : 0;
            
            if (finalMoney >= targetDebtGoal)
            {
                // GOOD ENDING
                if (finalGameWinPanel != null) finalGameWinPanel.SetActive(true);
                if (summaryMessageText != null) 
                    summaryMessageText.text = $"GOOD ENDING\n\nYou earned ${finalMoney} and cleared your debt. You are healthy, but life remains a stressful grind under your broker.";
            }
            else
            {
                // DEPORTED ENDING 1 (RUN OUT OF MONEY)
                if (finalGameLosePanel != null) finalGameLosePanel.SetActive(true);
                if (summaryMessageText != null) 
                    summaryMessageText.text = $"DEPORTED (DEBT FAILURE)\n\nYou failed to reach the required ${targetDebtGoal} to clear your debt layout.";
            }
        }
    }
    public void ClickToRestartEntireGame()
    {
        Time.timeScale = 1f; // Unfreeze time!
        // Reloads your current scene from scratch, wiping all memory completely fresh
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}