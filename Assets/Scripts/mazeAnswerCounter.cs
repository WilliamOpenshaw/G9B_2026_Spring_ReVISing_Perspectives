using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class mazeAnswerCounter : MonoBehaviour
{
    // 正確答案
    public string[] correctAnswer = new string[14];

    // 玩家答案
    public string[] userChoices = new string[14];

    public GameObject nextWinScreen;
    public GameObject failScreen;
    public GameObject currentScreen;

    // 14個勾勾位置
    public Image[] answerMarks;

    // 勾勾圖片
    public Sprite checkSprite;

    // 叉叉圖片
    public Sprite crossSprite;

    public int index = 0;

    void Start()
    {
        SetupAnswers();
        HideAllMarks();
    }

    void OnEnable()
    {
        SetupAnswers();
        HideAllMarks();
    }

    void SetupAnswers()
    {
        correctAnswer = new string[]
        {
            "buying",
            "cooking",
            "medicine",
            "cleaning",
            "washing",
            "cooking",
            "medicine",
            "dishes",
            "folding",
            "garbage",
            "cooking",
            "medicine",
            "shower",
            "book"
        };

        userChoices = new string[14];

        index = 0;
    }

    void HideAllMarks()
    {
        foreach (Image mark in answerMarks)
        {
            if (mark != null)
            {
                mark.gameObject.SetActive(false);
            }
        }
    }

    public void addToUserChoices(string choice)
    {
        if (index >= userChoices.Length)
            return;

        userChoices[index] = choice;

        // 顯示勾勾或叉叉
        if (answerMarks[index] != null)
        {
            if (choice == correctAnswer[index])
            {
                answerMarks[index].sprite = checkSprite;
            }
            else
            {
                answerMarks[index].sprite = crossSprite;
            }

            answerMarks[index].gameObject.SetActive(true);
        }

        index++;

        // 全部答完
        if (index == userChoices.Length)
        {
            if (userChoices.SequenceEqual(correctAnswer))
            {
                Debug.Log("All answers are correct!");

                if (nextWinScreen != null)
                    nextWinScreen.SetActive(true);

                if (currentScreen != null)
                    currentScreen.SetActive(false);
            }
            else
            {
                Debug.Log("Some answers are incorrect!");

                if (failScreen != null)
                    failScreen.SetActive(true);

                if (currentScreen != null)
                    currentScreen.SetActive(false);
            }

            index = 0;
        }
    }
}