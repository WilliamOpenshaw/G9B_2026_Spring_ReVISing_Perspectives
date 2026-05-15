using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        SetMaxHealth(100);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

    }

    public void SetHealth(int health)
    {
        slider.value = health; 

    }

    void Update()
    {
        /*
        // press e to lose 1 heatlh
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetHealth((int)slider.value - 10);
        }
        */


    }

}
