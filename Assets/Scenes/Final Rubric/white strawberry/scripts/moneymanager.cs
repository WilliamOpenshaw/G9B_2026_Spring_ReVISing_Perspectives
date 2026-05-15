using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class moneymanager : MonoBehaviour
{
    public TMP_Text moneyText;
    public int startMoney = -1000;
    public int currentMoney;

    void Awake()
    {
        currentMoney = startMoney;
    }

    void Start()
    {
        if (moneyText == null)
            Debug.LogWarning("moneymanager: moneyText is not assigned in the Inspector.");

        UpdateMoneyText();
    }

    public void AddMoney(int amount = 1)
    {
        currentMoney += amount;
        UpdateMoneyText();
        Debug.Log($"AddMoney called: {amount}. Current money = {currentMoney}");
    }

    public void SubtractMoney(int amount = 1)
    {
        currentMoney -= amount;
        UpdateMoneyText();
        Debug.Log($"SubtractMoney called: {amount}. Current money = {currentMoney}");
    }

    void UpdateMoneyText()
    {
        if (moneyText != null)
            moneyText.text = "$" + currentMoney.ToString();
    }
}