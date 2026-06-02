using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class EndGameBranchManager : MonoBehaviour
{
    [Header("Branching Panels")]
    public GameObject crossroadsPanel;
    public GameObject hospitalPanel;
    public GameObject policeStationPanel;

    [Header("Sign Button Text Fields")]
    public TextMeshProUGUI signAText;
    public TextMeshProUGUI signBText;

    [Header("HUD Elements to Hide")]
    public GameObject healthBarHUD; // Drag your UI Health bar/slider object here
    public GameObject moneyBarHUD;  // Drag your UI Money text/bar object here

    // List of alien/gibberish foreign words to simulate a language barrier
    private List<string> incomprehensibleSigns = new List<string>() 
    { 
        "HGV-76", "ØXØ-M", "KÆR-9", "PLØN", "Zêta-4", "VOSS-X", "Ω-99", "Ẍ-2" 
    };

    private bool signALeadsToHospital;

    // This gets called by the DayLoopManager if your health is under 20 at the end of Day 3
    public void LaunchCrossroadsGamble()
    {
        if (healthBarHUD != null) healthBarHUD.SetActive(false);
        if (moneyBarHUD != null) moneyBarHUD.SetActive(false);
        if (crossroadsPanel != null) crossroadsPanel.SetActive(true);

        // 1. Randomly decide if Sign A is the safe route (Hospital) or trap route (Police)
        signALeadsToHospital = (Random.value > 0.5f);

        // 2. Grab two random gibberish words from our list so they change every single time
        int firstRandomIndex = Random.Range(0, incomprehensibleSigns.Count);
        int secondRandomIndex = Random.Range(0, incomprehensibleSigns.Count);
        
        // Prevent picking the exact same word for both signs
        while (secondRandomIndex == firstRandomIndex)
        {
            secondRandomIndex = Random.Range(0, incomprehensibleSigns.Count);
        }

        // 3. Print the dynamic gibberish onto the buttons
        if (signAText != null) signAText.text = incomprehensibleSigns[firstRandomIndex];
        if (signBText != null) signBText.text = incomprehensibleSigns[secondRandomIndex];

        Debug.Log($"[Gamble Configured] Does Sign A lead to safety? -> {signALeadsToHospital}");
    }

    // Connect this to SignButtonA's OnClick() event in Unity
    public void ChooseSignA()
    {
        if (crossroadsPanel != null) crossroadsPanel.SetActive(false);

        if (signALeadsToHospital)
        {
            TriggerHospitalPath();
        }
        else
        {
            TriggerPolicePath();
        }
    }

    // Connect this to SignButtonB's OnClick() event in Unity
    public void ChooseSignB()
    {
        if (crossroadsPanel != null) crossroadsPanel.SetActive(false);

        // If A was the hospital, B must be the police (and vice versa)
        if (!signALeadsToHospital)
        {
            TriggerHospitalPath();
        }
        else
        {
            TriggerPolicePath();
        }
    }

    void TriggerHospitalPath()
    {
        Debug.Log("Path Chosen: Hospital.");
        if (hospitalPanel != null) hospitalPanel.SetActive(true);
    }

    void TriggerPolicePath()
    {
        Debug.Log("Path Chosen: Police Station.");
        if (policeStationPanel != null) policeStationPanel.SetActive(true);
    }

    // Connect this to the Restart Buttons on your final screens
    // Inside EndGameBranchManager.cs
    public void RestartEntireGame()
    {
        Debug.Log("Master Reset Initiated! Scrubbing all game memory...");

        // 1. Unfreeze time so the game can actually update again
        Time.timeScale = 1f; 

        // 2. Find the Day Loop Manager and force it back to Day 1, baseline money/debt, etc.
        DayLoopManager dayManager = FindFirstObjectByType<DayLoopManager>();
        if (dayManager != null)
        {
            dayManager.currentDay = 1;
            
            // Re-enable the main gameplay elements container if it was hidden
            if (dayManager.day3GameplayParent != null) 
                dayManager.day3GameplayParent.SetActive(true);
            
            // Reset money if your script has a reset method
            if (dayManager.realMoneyScript != null) 
                dayManager.realMoneyScript.currentMoney = 0; 

            // Reset health back to a fresh 100%
            if (dayManager.realHealthScript != null && dayManager.realHealthScript.slider != null)
                dayManager.realHealthScript.slider.value = 100f;
        }

        // 3. Find and wipe the laundry sorting game variables
        LaundrySortingGame laundryScript = FindFirstObjectByType<LaundrySortingGame>();
        if (laundryScript != null)
        {
            laundryScript.ResetMiniGameForNewDay();
            laundryScript.ResetLaundryProgressForNewDay();
        }

        // 4. Find and wipe the cooking mini game variables
        CookingGame cookingScript = FindFirstObjectByType<CookingGame>();
        if (cookingScript != null)
        {
            cookingScript.ResetMiniGameForNewDay();
        }

        // 5. Turn off all the endgame panels so they don't block the screen on reload
        if (crossroadsPanel != null) crossroadsPanel.SetActive(false);
        if (hospitalPanel != null) hospitalPanel.SetActive(false);
        if (policeStationPanel != null) policeStationPanel.SetActive(false);

        // 6. FINALLY, reload the scene fresh with empty memory slots!
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
        
        
    }
}