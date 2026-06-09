using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class healthbar : MonoBehaviour
{
    public Slider slider;
    public TMPro.TextMeshProUGUI healthText;

    void Start()
    {
        ApplyDifficultyHealth();
    }

    private void ApplyDifficultyHealth()
    {
        if (DifficultyManager.CurrentMode == DifficultyManager.GameMode.Baby) {
            slider.maxValue = 130;
            slider.value = 130;
        }
        else if (DifficultyManager.CurrentMode == DifficultyManager.GameMode.Easy) {
            slider.maxValue = 110;
            slider.value = 110;
        }
        else {
            slider.maxValue = 100;
            slider.value = 100;
        }

        // FIX THE "NEW TEXT" BUG: Force the text to display the new number immediately!
        if (healthText != null)
        {
            healthText.text = slider.value.ToString();
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        if (healthText != null) healthText.text = health.ToString();
    }

    public void SetHealth(int health)
    {
        slider.value = health; 
        if (healthText != null) healthText.text = health.ToString();
    }

    public void ReduceHealthBy(int amount)
    {
        SetHealth((int)slider.value - amount);
    }

    // FIXED: Instead of forcing 100 HP blindly, it checks your difficulty setting again!
    public void ResetHealth()
    {
        ApplyDifficultyHealth();
    }
}