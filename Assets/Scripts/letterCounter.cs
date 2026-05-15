using UnityEngine;

public class letterCounter : MonoBehaviour
{
    // #
    // true false
    // 4 true false bools for gameobjects
    public bool letter1opened = false;
    public bool letter2opened = false;
    public bool letter3opened = false;
    public bool letter4opened = false;

    // 4 gameobjects for found and not found
    public GameObject letter1Found;
    public GameObject letter1NotFound;
    public GameObject letter2Found;
    public GameObject letter2NotFound;
    public GameObject letter3Found;
    public GameObject letter3NotFound;
    public GameObject letter4Found;
    public GameObject letter4NotFound;

    public GameObject outcomeWin;
    public GameObject outcomeLose;
    public GameObject choiceSlide;

    public void checkLetters()
    {
        if (letter1opened == true && 
            letter2opened == true && 
            letter3opened == true && 
            letter4opened == true)
        {
            Debug.Log("All letters opened!");
            outcomeWin.SetActive(true);
            choiceSlide.SetActive(false);
        }
    }

    public void changeBool(int letterNumber)
    {
        switch (letterNumber)
        {
            case 1:
                letter1opened = true;
                break;
            case 2:
                letter2opened = true;
                break;
            case 3:
                letter3opened = true;
                break;
            case 4:
                letter4opened = true;
                break;
            default:
                Debug.Log("Invalid letter number");
                break;
        }
        checkLetters();
    }

    public void checkIfAlreadyOpened(int letterNumber)
    {
        switch (letterNumber)
        {
            case 1:
                if (letter1opened)
                {
                    choiceSlide.SetActive(false);
                    letter1Found.SetActive(false);
                    letter1NotFound.SetActive(true);
                }
                break;
            case 2:
                if (letter2opened)
                {
                    choiceSlide.SetActive(false);
                    letter2Found.SetActive(false);
                    letter2NotFound.SetActive(true);
                }
                break;
            case 3:
                if (letter3opened)
                {
                    choiceSlide.SetActive(false);
                    letter3Found.SetActive(false);
                    letter3NotFound.SetActive(true);
                }
                break;
            case 4:
                if (letter4opened)
                {
                    choiceSlide.SetActive(false);
                    letter4Found.SetActive(false);
                    letter4NotFound.SetActive(true);
                }
                break;
            default:
                Debug.Log("Invalid letter number");
                break;
        }
    }
    
}
