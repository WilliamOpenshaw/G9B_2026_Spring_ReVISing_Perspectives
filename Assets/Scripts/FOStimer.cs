using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FOStimer : MonoBehaviour
{
    public float timeRemaining = 100; // Changed to 100 seconds
    public bool timerIsRunning = false; 
    public TextMeshProUGUI timeText;

    public GameObject loseScreen;

    // Safety lock to prevent accidental auto-start
    private bool isTimerActivated = false;

    void Start()
    {
        // Force the timer to stay off when entering Playmode
        timerIsRunning = false;
        isTimerActivated = false;
        DisplayTime(timeRemaining);
    }

    void FixedUpdate()
    {
        // Only run if it's both running AND explicitly activated by the player
        if (timerIsRunning && isTimerActivated)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeText.text = "00:00";
                timeRemaining = 0;
                timerIsRunning = false;
                isTimerActivated = false;
                
                if (loseScreen != null) loseScreen.SetActive(true);
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay);
        float milliseconds = Mathf.FloorToInt((timeToDisplay - seconds) * 100);
        timeText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);
    }

    // Call this function to explicitly unlock and start the countdown
    public void StartTimer()
    {
        isTimerActivated = true;
        timerIsRunning = true;
    }

    public void StopTimer()
    {
        timerIsRunning = false;
        isTimerActivated = false;
    }

    // NEW LOOPING FUNCTION ADDED BELOW:
    public void ResetTimer()
    {
        timeRemaining = 100f; // Resets clock back to your full 100 seconds
        timerIsRunning = false; 
        isTimerActivated = false;
        DisplayTime(timeRemaining);
    }
}