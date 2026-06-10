using UnityEngine;

public class RestartMaze : MonoBehaviour
{
    public mazeGameManager gameManager;
    public mazeTimer2 timer;

    public GameObject mazeScreen;
    public GameObject winScreen;
    public GameObject loseScreen;

    public Transform player;
    public Transform startPoint;

    public void RestartGame()
    {
        gameManager.ResetGame();

        if (timer != null)
            timer.ResetTimer();

        if (player != null && startPoint != null)
            player.position = startPoint.position;

        if (winScreen != null)
            winScreen.SetActive(false);

        if (loseScreen != null)
            loseScreen.SetActive(false);

        if (mazeScreen != null)
            mazeScreen.SetActive(true);
    }
}