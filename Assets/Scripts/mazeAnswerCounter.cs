using UnityEngine;
using System.Linq;

public class mazeAnswerCounter : MonoBehaviour
{
    // answer list
    public string[] correctAnswer = new string[14];
    // user choices list empty for now, will be filled with user choices in the future
    public string[] userChoices = new string[14];

    public GameObject nextWinScreen; // Reference to the next win screen GameObject
    public GameObject failScreen; // Reference to the fail screen GameObject

    public GameObject currentScreen; // Reference to the current screen GameObject
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        correctAnswer = new string[] { "buying", 
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
                                "book" };
        // user choices will be filled with user choices in the future, for now it is empty
        userChoices = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
    }

    void OnEnable()
    {
        correctAnswer = new string[] { "buying", 
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
                                "book" };
        // user choices will be filled with user choices in the future, for now it is empty
        userChoices = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
    }


    // function to take string and add it to userchoices list
    // find next empty slot in userchoices list and add the string to it
    public int index = 0;
    public void addToUserChoices(string choice)
    {
        if (index < userChoices.Length)
        {
            userChoices[index] = choice;
            index++;
        }
        // check if userchoices list is full of non-empty strings
        if (index == userChoices.Length)
        {
            // if it is, compare it to correctAnswer list and if all is correct and order matches, enable button gameobject
            bool allCorrect = false;
            if (userChoices.SequenceEqual(correctAnswer))
            {
                allCorrect = true;
                nextWinScreen.SetActive(true);
                userChoices = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                currentScreen.SetActive(false);
                Debug.Log("All answers are correct!");
                //Debug.Log(userChoices);
                //Debug.Log(correctAnswer);
                Debug.Log(userChoices.SequenceEqual(correctAnswer));
                index = 0;
            }
            else
            {
                // enable fail gameobject
                failScreen.SetActive(true);
                userChoices = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                currentScreen.SetActive(false);
                Debug.Log("Some answers are incorrect!");
                //Debug.Log(userChoices);
                //Debug.Log(correctAnswer);
                Debug.Log(userChoices.SequenceEqual(correctAnswer));
                index = 0;
            }
        }
    }


}
