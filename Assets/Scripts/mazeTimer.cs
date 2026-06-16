using UnityEngine;
using TMPro;

public class mazeTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public int startSeconds = 40;

    private int seconds;
    private int milliseconds;

    public bool playerHasWon = false;

    void Start()
    {
        ResetTimer();
    }

    void OnEnable()
    {
        ResetTimer();
    }

    void FixedUpdate()
    {
        if (playerHasWon)
            return;

        if (seconds > 0)
        {
            milliseconds -= (int)(Time.deltaTime * 100) * 2;

            if (milliseconds < 0)
            {
                seconds--;
                milliseconds = 99;
            }

            if (timerText != null)
            {
                timerText.text =
                    seconds.ToString("00") +
                    ":" +
                    milliseconds.ToString("00");
            }
        }
    }

    public void ResetTimer()
    {
        seconds = startSeconds;
        milliseconds = 0;
        playerHasWon = false;

        if (timerText != null)
        {
            timerText.text =
                seconds.ToString("00") +
                ":" +
                milliseconds.ToString("00");
        }

        Debug.Log("TIMER RESET");
    }
}