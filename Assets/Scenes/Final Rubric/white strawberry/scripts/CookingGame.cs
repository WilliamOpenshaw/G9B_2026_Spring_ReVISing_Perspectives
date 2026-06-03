using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CookingGame : MonoBehaviour
{
    [Header("Game Panels")]
    public GameObject winPanel; // Drag your WinPanel here
    public GameObject FailPanel;
    public GameObject ingredientPanel;     
    public GameObject stovePanel;          
    public GameObject popupPanel;    

    [Header("Recipe Progression Settings")]
    // Active recipe running right now
    private List<string> recipeOrder = new List<string>();
    private int currentIngredientIndex = 0;
    private bool isShelfPhaseComplete = false;

    private int healthCostForStoryPenalty = 50; 
    private int totalDishesToWin = 3;

    // HAND-CRAFTED 3-DISH MENU
    private List<string> dish1Recipe = new List<string> { "Onion", "Oil", "Rice" };
    private List<string> dish2Recipe = new List<string> { "Water", "Onion", "Egg" };
    private List<string> dish3Recipe = new List<string> { "Oil", "Onion", "Meat", "Noodles" };

    [Header("UI Elements")]
    public TextMeshProUGUI recipeListText;
    public TextMeshProUGUI popupText;      
    public TextMeshProUGUI timerText;      
    public Slider cookingSlider;
    public GameObject continueButton;           

    [Header("Global Timer Settings")]
    public float totalGameTime = 60f;      
    private float currentTimer;
    private bool isGameActive = true;
    private bool isCookingPhaseActive = false;

    [Header("Dishes Progress")]
    private int completedDishes = 0;
    private const int REQUIRED_DISHES = 3;

    [Header("Stove Slider Math")]
    private float sliderMinGreen = 0.65f;  
    private float sliderMaxGreen = 0.85f;  
    private float sliderSpeed = 2f;        
    private bool movingUp = true;          

    void OnEnable()
    {
        // EMERGENCY SAFETY FIX: Force all input gates open and clear old blocking popups
        isGameActive = true;             
        isCookingPhaseActive = false;
        isShelfPhaseComplete = false;
        currentIngredientIndex = 0;
        completedDishes = 0;

        if (popupPanel != null) popupPanel.SetActive(false); 
        if (winPanel != null) winPanel.SetActive(false);
        if (continueButton != null) continueButton.SetActive(false);

        // 2. DYNAMIC DIFFICULTY SCALE: Set up dishes, speeds, and the story end penalties
        if (DifficultyManager.CurrentMode == DifficultyManager.GameMode.Baby) // Easy Mode Button
        {
            totalDishesToWin = 1;
            sliderSpeed = 1.0f;           // Super slow and easy slider
            healthCostForStoryPenalty = 0; // No onion allergy penalty! Family is totally happy
        }
        else if (DifficultyManager.CurrentMode == DifficultyManager.GameMode.Easy) // Medium Mode Button
        {
            totalDishesToWin = 2;
            sliderSpeed = 1.5f;            // Noticeably slower, manageable speed
            healthCostForStoryPenalty = 30; // Mild story penalty
        }
        else // Hard Mode
        {
            totalDishesToWin = 3;
            sliderSpeed = 2.5f;            // Original challenging fast speed
            healthCostForStoryPenalty = 50; // The brutal onion allergy 50 HP story hit
        }

        currentTimer = totalGameTime; // Reset clock
        LoadDishRecipe(1);            // Start dish 1

        if (ingredientPanel != null) ingredientPanel.SetActive(true);
        if (stovePanel != null) stovePanel.SetActive(false);

        UpdateRecipeListUI();
        Debug.Log($"Kitchen loaded on {DifficultyManager.CurrentMode}! Dishes needed: {totalDishesToWin}, Speed: {sliderSpeed}");
    }

    // Helper method to completely assign the ingredients based on current dish index
    void LoadDishRecipe(int dishNumber)
    {
        currentIngredientIndex = 0;
        isShelfPhaseComplete = false;
        isCookingPhaseActive = false;

        if (dishNumber == 1) recipeOrder = new List<string>(dish1Recipe);
        else if (dishNumber == 2) recipeOrder = new List<string>(dish2Recipe);
        else if (dishNumber == 3) recipeOrder = new List<string>(dish3Recipe);

        Debug.Log($"Loaded recipe configuration for Dish #{dishNumber}!");
    }

    void Update()
    {
        if (!isGameActive) return;

        HandleGlobalTimer();

        if (isCookingPhaseActive)
        {
            HandleSliderMovement();
        }
    }

    void HandleGlobalTimer()
    {
        // 1. EMERGENCY SAFETY GATES: Stop counting if time is already up or game is inactive
        if (currentTimer <= 0 || !isGameActive) 
        {
            timerText.text = "Time Left: <color=red>0:00</color>";
            return; 
        }

        // 2. Countdown normally
        currentTimer -= Time.deltaTime;

        // 3. FORCE clamp so the math can NEVER drop below absolute zero
        if (currentTimer < 0) currentTimer = 0;

        int minutes = Mathf.FloorToInt(currentTimer / 60);
        int seconds = Mathf.FloorToInt(currentTimer % 60);
        
        // 4. Update the text UI safely
        timerText.text = $"Time Left: <color=red>{minutes}:{seconds:D2}</color>";

        // 5. Trigger Game Over cleanly at exactly 0
        if (currentTimer <= 0)
        {
            GameOver(false); 
        }
    }

    #region INGREDIENT PHASE
    public void ClickIngredient(string ingredientName)
    {
        if (!isGameActive || isCookingPhaseActive || popupPanel.activeSelf) return;

        if (ingredientName == recipeOrder[currentIngredientIndex])
        {
            currentIngredientIndex++; 
            UpdateRecipeListUI(); 

            if (currentIngredientIndex >= recipeOrder.Count)
            {
                isShelfPhaseComplete = true;
                SwitchToStovePhase();
            }
        }
        else
        {
            StartCoroutine(TriggerPenalty(ingredientName));
        }
    }

    void UpdateRecipeListUI()
    {
        // Give each dish a readable title header based on completion count
        string currentDishTitle = "Fried Rice";
        if (completedDishes == 1) currentDishTitle = "Egg Drop Soup";
        else if (completedDishes == 2) currentDishTitle = "Stir-Fry Noodles";

        recipeListText.text = $"<b>Dishes: {completedDishes}/{totalDishesToWin}</b>\n";
        recipeListText.text += $"<b>Making: <color=#4A2306>{currentDishTitle}</color></b>\n\n";
        recipeListText.text += "<b>Ingredients Needed:</b>\n";

        for (int i = 0; i < recipeOrder.Count; i++)
        {
            if (i < currentIngredientIndex) recipeListText.text += $"<s><color=#1E5631>{recipeOrder[i]}</color></s>\n";
            else if (i == currentIngredientIndex) recipeListText.text += $"<color=#1A1A1A>👉 {recipeOrder[i]}</color>\n";
            else recipeListText.text += $"{recipeOrder[i]}\n";
        }
    }

    System.Collections.IEnumerator TriggerPenalty(string wrongItem)
    {
        popupText.text = $"Wrong item! You grabbed <b>{wrongItem}</b> instead of <b>{recipeOrder[currentIngredientIndex]}</b>.\n\n<i>Waiting 3 seconds...</i>";
        popupPanel.SetActive(true); 
        
        yield return new WaitForSeconds(3f); 
        
        // SAFETY CHECK: Only turn off the popup if the game hasn't already timed out!
        if (isGameActive)
        {
            popupPanel.SetActive(false); 
        }
    }
    #endregion

    #region STOVE PHASE
    void SwitchToStovePhase()
    {
        ingredientPanel.SetActive(false);
        stovePanel.SetActive(true);
        isCookingPhaseActive = true;
        cookingSlider.value = 0f;
    }

    void HandleSliderMovement()
    {
        if (movingUp)
        {
            cookingSlider.value += Time.deltaTime * sliderSpeed;
            if (cookingSlider.value >= 1f) movingUp = false;
        }
        else
        {
            cookingSlider.value -= Time.deltaTime * sliderSpeed;
            if (cookingSlider.value <= 0f) movingUp = true;
        }
    }

    public void ClickCookButton()
    {
        if (!isCookingPhaseActive) return;

        if (cookingSlider.value >= sliderMinGreen && cookingSlider.value <= sliderMaxGreen)
        {
            completedDishes++;
            
            if (completedDishes >= totalDishesToWin)
            {
                GameOver(true); 
            }
            else
            {
                ResetForNextRound(); 
            }
        }
        else
        {
            StartCoroutine(TriggerStoveFailureMessage());
        }
    }

    void ResetForNextRound()
    {
        isCookingPhaseActive = false;
        stovePanel.SetActive(false);
        ingredientPanel.SetActive(true);
        
        sliderSpeed += 0.6f; // Make the bar move slightly faster each dish!       

        // Load the next recipe set based on the updated dish complete count
        LoadDishRecipe(completedDishes + 1);

        UpdateRecipeListUI();
    }

    System.Collections.IEnumerator TriggerStoveFailureMessage()
    {
        isCookingPhaseActive = false; 
        popupText.text = "<b>Food Burned!</b>\nYou missed the green zone.\n\n<i>Try again in 2 seconds...</i>";
        popupPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        popupPanel.SetActive(false);
        
        cookingSlider.value = 0f; 
        movingUp = true;
        isCookingPhaseActive = true; 
    }
    #endregion

    void GameOver(bool success)
    {
        isGameActive = false;
        isCookingPhaseActive = false;
        
        ingredientPanel.SetActive(false);
        stovePanel.SetActive(false);

        if (success) 
    {
        winPanel.SetActive(true);
        popupPanel.SetActive(false); 
        if (continueButton != null) continueButton.SetActive(false); 

        // EMERGENCY HEALTH ROUTER
        healthbar playerHealth = FindFirstObjectByType<healthbar>(FindObjectsInactive.Include);

        // Backup plan: If it still can't find it by type, try finding it by its GameObject name
        if (playerHealth == null)
        {
            GameObject hbObj = GameObject.Find("healthbar") ?? GameObject.Find("HealthBar") ?? GameObject.Find("Health Bar");
            if (hbObj != null)
            {
                playerHealth = hbObj.GetComponent<healthbar>();
            }
        }

        // Apply the penalty if we successfully located the health bar
        if (playerHealth != null)
        {
            playerHealth.ReduceHealthBy(healthCostForStoryPenalty);
            Debug.Log($"Cooking penalty applied successfully: -{healthCostForStoryPenalty} HP");
        }
        else
        {
            // CRITICAL DEBUGGER: If it still fails, this will tell us exactly why in the console!
            Debug.LogError("CRITICAL: Cooking Game won, but could NOT find the healthbar object in your scene layout!");
        }
    }
        else
        {
            popupPanel.SetActive(false);
            winPanel.SetActive(false);
            continueButton.SetActive(false);
            FailPanel.SetActive(true); 
            
            popupText.text = "<b>GAME OVER!</b>\nYou ran out of time. The family is angry.";
            Debug.Log("Game ended: PLAYER LOST!");
            healthbar playerHealth = FindFirstObjectByType<healthbar>();
        if (playerHealth != null)
        {
            playerHealth.ReduceHealthBy(healthCostForStoryPenalty);
            Debug.Log($"Cooking penalty applied cleanly: -{healthCostForStoryPenalty} HP");
        }
        else
        {
            Debug.LogError("CRITICAL: Cooking Game lost, but could NOT find the healthbar object in your scene layout!");
        }
        }
    }
    

    public void ResetMiniGameForNewDay()
    {
        completedDishes = 0;
        sliderSpeed = 2f; 
        currentTimer = totalGameTime;
        isGameActive = true;

        // Force back to Dish 1 layout data
        LoadDishRecipe(1);

        ingredientPanel.SetActive(true);
        stovePanel.SetActive(false);
        popupPanel.SetActive(false);
        winPanel.SetActive(false);
        if (continueButton != null) continueButton.SetActive(false);

        UpdateRecipeListUI();

        Debug.Log("Cooking Game has been completely reset for the new day!");
    }
}