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
        SetMaxHealth(100);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthText.text = health.ToString();

    }

    public void SetHealth(int health)
    {
        slider.value = health; 
        healthText.text = health.ToString();

    }

    public void ReduceHealthBy20()
    {
        SetHealth((int)slider.value - 20);
    }

    public void ResetHealth()
    {
        SetMaxHealth(100);
    }

    // Removed keyboard input, now using UI button

}
