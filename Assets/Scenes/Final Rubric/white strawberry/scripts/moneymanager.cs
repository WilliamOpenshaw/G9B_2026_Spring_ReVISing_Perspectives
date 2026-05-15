using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class moneymanager : MonoBehaviour
{
    public TMP_Text moneyText;
    public int currentMoney = 0;

    void Start()
    {
        UpdateMoneyText();
    }

    public void AddMoney(int amount = 1)
    {
        currentMoney += amount;
        UpdateMoneyText();
    }

    public void SubtractMoney(int amount = 1)
    {
        currentMoney -= amount;
        if (currentMoney < 0)
            currentMoney = 0;
        UpdateMoneyText();
    }

    void UpdateMoneyText()
    {
        if (moneyText != null)
            moneyText.text = "Money: " + currentMoney.ToString();
    }
}