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
        // Give health back using your health script's built-in function!
        if (canSleepAndHeal && realHealthScript != null && realHealthScript.slider != null)
        {
            int currentHP = (int)realHealthScript.slider.value;
            int newHP = currentHP + 10;
            if (newHP > 100) newHP = 100;

            realHealthScript.SetHealth(newHP); 
        }

        // Close the screen summary box so they can see the game again
        if (endOfDaySummaryPanel != null) endOfDaySummaryPanel.SetActive(false);

        // ========================================================
        // FRESH RESET: WIPE MINI-GAME PROGRESS BEFORE ADVANCING THE DAY!
        // ========================================================
        CookingGame cookingScript = Object.FindFirstObjectByType<CookingGame>();
        if (cookingScript != null)
        {
            cookingScript.ResetMiniGameForNewDay();
        }

        CleaningGame cleaningScript = Object.FindFirstObjectByType<CleaningGame>();
        if (cleaningScript != null)
        {
            cleaningScript.ResetMiniGameForNewDay();
        }
        LaundrySortingGame laundryScript = Object.FindFirstObjectByType<LaundrySortingGame>();
        if (laundryScript != null) laundryScript.ResetMiniGameForNewDay();

    

        // Advance to the next day
        currentDay++;

        // DYNAMIC ROUTING: Turn on the correct day based on the new number!
        if (currentDay == 2)
        {
            if (day1GameplayParent != null) day1GameplayParent.SetActive(false); // Turn off Day 1
            if (day2GameplayParent != null) day2GameplayParent.SetActive(true);  // Turn on Day 2
            Debug.Log("Welcome to Day 2!");
        }
        else if (currentDay == 3)
        {
            if (day2GameplayParent != null) day2GameplayParent.SetActive(false); // Turn off Day 2
            if (day3GameplayParent != null) day3GameplayParent.SetActive(true);  // Turn on Day 3
            Debug.Log("Welcome to Day 3!");
        }
        else if (currentDay > 3)
        {
            // If they sleep past Day 3, show the final game over or win screen!
            int finalMoney = (realMoneyScript != null) ? realMoneyScript.currentMoney : 0;
            
            if (finalMoney >= targetDebtGoal)
            {
                summaryMessageText.text = "YOU WIN!\n\nYou paid off your debt and survived the loop.";
            }
            else
            {
                summaryMessageText.text = "GAME OVER\n\nYou ran out of time to clear your debt.";
            }
            
            if (sleepButtonLabelText != null) sleepButtonLabelText.text = "...";
            if (endOfDaySummaryPanel != null) endOfDaySummaryPanel.SetActive(true);
        }
    }
}