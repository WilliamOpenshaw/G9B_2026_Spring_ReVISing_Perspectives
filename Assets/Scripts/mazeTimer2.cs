using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class mazeTimer2 : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component to display the timer
    public TextMeshProUGUI timerText;
    // variables for keeping second and milliseconds on a countdown 1 minute timer
    public int seconds = 40;
    private int milliseconds = 0;

    public GameObject winScreen; // Reference to the win screen GameObject
    public GameObject loseScreen; // Reference to the lose screen GameObject

    public GameObject currentScreen;

    public bool playerHasWon = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        // update the timer text to show the initial time
        timerText.text = seconds.ToString("00") + ":" + milliseconds.ToString("00");
        playerHasWon = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Update the timer every frame
        if (seconds > 0 && playerHasWon == false)
        {
            // Decrease milliseconds by the time passed since the last frame
            milliseconds -= (int)(Time.deltaTime * 100) * 2;
            // If milliseconds are less than 0, decrease seconds and reset milliseconds to 99
            if (milliseconds < 0)
            {
                seconds--;
                milliseconds = 99;
            }
            // Update the timer text to show the current time
            timerText.text = seconds.ToString("00") + ":" + milliseconds.ToString("00");
        }
        else if (playerHasWon)
        {
            // If the player has won, display "You Win!" and stop updating
            timerText.text = "You Win!";
            winScreen.SetActive(true); // Show the win screen
            currentScreen.SetActive(false); // Hide the current screen

        }
        else
        {
            // If the timer reaches zero, display "Time's Up!" and stop updating
            timerText.text = "Time's Up!";
            loseScreen.SetActive(true); // Show the lose screen
            currentScreen.SetActive(false); // Hide the current screen

        }
    }

    public void ResetTimer()
    {
        // Reset the timer to the initial values
        seconds = 40;
        milliseconds = 0;
        playerHasWon = false;
        timerText.text = seconds.ToString("00") + ":" + milliseconds.ToString("00");
    }
}
