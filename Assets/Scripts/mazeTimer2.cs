using UnityEngine;
using TMPro;

public class mazeTimer2 : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float timeRemaining = 120f;

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject currentScreen;

    public bool playerHasWon = false;

    void onEnable()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        // update the timer text to show the initial time
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        playerHasWon = false;

    }

    void Update()
    {
        if (!playerHasWon && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else if (playerHasWon)
        {
            timerText.text = "You Win!";
            winScreen.SetActive(true);
            currentScreen.SetActive(false);
        }
        else
        {
            timerText.text = "Time's Up!";
            loseScreen.SetActive(true);
            currentScreen.SetActive(false);
        }
    }

    public void ResetTimer()
    {
        timeRemaining = 120f;
        playerHasWon = false;
    }
}