using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CookingGame : MonoBehaviour
{
    [Header("Game Panels")]
    public GameObject ingredientPanel;     
    public GameObject stovePanel;          
    public GameObject popupPanel;          

    [Header("Recipe Settings")]
    public List<string> recipeOrder = new List<string> { "Onion", "Oil", "Rice" };
    private int currentIngredientIndex = 0;
    private bool isShelfPhaseComplete = false;

    [Header("UI Elements")]
    public TextMeshProUGUI recipeListText;
    public TextMeshProUGUI popupText;      
    public TextMeshProUGUI timerText;      
    public Slider cookingSlider;           

    [Header("Global Timer Settings")]
    public float totalGameTime = 40f;      
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

    void Start()
    {
        currentTimer = totalGameTime;
        
        ingredientPanel.SetActive(true);
        stovePanel.SetActive(false);
        popupPanel.SetActive(false);

        UpdateRecipeListUI();
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
        currentTimer -= Time.deltaTime;
        timerText.text = $"Time Left: <color=red>{currentTimer:F1}s</color>";

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
        recipeListText.text = $"<b>Dishes: {completedDishes}/{REQUIRED_DISHES}</b>\n\n";
        recipeListText.text += "<b>Ingredients Needed:</b>\n";
        for (int i = 0; i < recipeOrder.Count; i++)
        {
            if (i < currentIngredientIndex) recipeListText.text += $"<s><color=green>{recipeOrder[i]}</color></s>\n";
            else if (i == currentIngredientIndex) recipeListText.text += $"<color=yellow>👉 {recipeOrder[i]}</color>\n";
            else recipeListText.text += $"{recipeOrder[i]}\n";
        }
    }

    System.Collections.IEnumerator TriggerPenalty(string wrongItem)
    {
        popupText.text = $"Wrong item! You grabbed <b>{wrongItem}</b> instead of <b>{recipeOrder[currentIngredientIndex]}</b>.\n\n<i>Waiting 3 seconds...</i>";
        popupPanel.SetActive(true); 
        yield return new WaitForSeconds(3f); 
        popupPanel.SetActive(false); 
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
            
            if (completedDishes >= REQUIRED_DISHES)
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
        
        isShelfPhaseComplete = false;
        currentIngredientIndex = 0; 
        sliderSpeed += 0.6f;        

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
        popupPanel.SetActive(true);

        if (success) popupText.text = "<b>SUCCESS!</b>\nYou managed to cook all 3 meals before your shift ended!";
        else popupText.text = "<b>GAME OVER!</b>\nYou ran out of time. The family is angry.";
    }
}