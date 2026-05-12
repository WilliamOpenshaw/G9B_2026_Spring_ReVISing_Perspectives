using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro

public class NameManager : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public string playerName;
//part2. put up public TextMeshProUGUI [] did it!! 


    public TextMeshProUGUI text1;
    // example: public TextMeshProUGUI text2;

    public void SavePlayerName()
    {
        // Store the text from the input field
        playerName = nameInputField.text;
        Debug.Log("Player Name Saved: " + playerName);
        SetDisplayOfPlayerName();
        
    }
    public void ResetPlayerName()
    {
        // Store the text from the input field
        playerName = "";
        nameInputField.text = "";
        Debug.Log("Player Name Saved: " + playerName);
        
    }
    public void SetDisplayOfPlayerName()
    {
        if (playerName == "")
        {
            playerName = "player";
        }
        
        //1. put the new text up so like text2.text = "abc " + playerName + " def" REMEMBER TO ADDTHE ; !! 
        text1.text = "Hello " + playerName + ", you must be the new worker! Welcome welcome. I will be your broker. Put your stuff down and get to work.";
        
        // example: text2.text = "abc " + playerName + " def";
        
        
    }
}
//everytime you want to add dialogue that need player name: heres what you do
//next thing you do, is to drag the dialogue to like where it should be
//example