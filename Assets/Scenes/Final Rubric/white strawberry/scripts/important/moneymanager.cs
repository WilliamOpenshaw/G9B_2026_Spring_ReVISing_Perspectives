using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class moneymanager : MonoBehaviour
{
    public TMP_Text moneyText;
    public int startMoney = 0;
    public int currentMoney;

    int lastMoney;

    void Awake()
    {
        currentMoney = startMoney;
        lastMoney = currentMoney;
    }

    bool hasWarnedNoText;

    void Start()
    {
        if (moneyText == null)
            moneyText = GetComponent<TMP_Text>();

        if (moneyText == null)
            moneyText = GetComponentInChildren<TMP_Text>();

        if (moneyText == null)
        {
            var allTexts = Object.FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);
            foreach (var text in allTexts)
            {
                if (text.gameObject.name.ToLower().Contains("money") || text.gameObject.name.ToLower().Contains("cash"))
                {
                    moneyText = text;
                    break;
                }
            }

            if (moneyText == null && allTexts.Length > 0)
                moneyText = allTexts[0];
        }

        if (moneyText == null)
        {
            Debug.LogWarning("moneymanager: moneyText is not assigned in the Inspector and no TMP_Text was found.");
            hasWarnedNoText = true;
        }
        else
        {
            Debug.Log("moneymanager: moneyText auto-assigned to " + moneyText.gameObject.name);
        }

        UpdateMoneyText();
    }

    void Update()
    {
        if (currentMoney != lastMoney)
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
        {
            moneyText.text = "$" + currentMoney.ToString();
            lastMoney = currentMoney;
        }
        else if (!hasWarnedNoText)
        {
            Debug.LogWarning("moneymanager: Cannot update money display because moneyText is missing.");
            hasWarnedNoText = true;
        }
    }
}