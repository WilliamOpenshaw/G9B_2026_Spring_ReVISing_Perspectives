using UnityEngine;
using TMPro;

public class mazeTimer2 : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float timeRemaining = 90f;

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject currentScreen;

    public bool playerHasWon = false;

    void OnEnable()
    {
        timeRemaining = 90f;
        playerHasWon = false;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    void Update()
    {
        if (playerHasWon)
        {
            return;
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else
        {
            timerText.text = "Time's Up!";

            if (loseScreen != null)
                loseScreen.SetActive(true);

            if (currentScreen != null)
                currentScreen.SetActive(false);
        }
    }

    public void PlayerWon()
    {
        playerHasWon = true;

        if (winScreen != null)
            winScreen.SetActive(true);

        if (currentScreen != null)
            currentScreen.SetActive(false);
    }

    public void ResetTimer()
    {
        timeRemaining = 90f;
        playerHasWon = false;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}