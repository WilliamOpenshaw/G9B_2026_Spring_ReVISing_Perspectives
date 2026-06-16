using UnityEngine;

public class resetListTimer : MonoBehaviour
{
    public mazeTimer timer;

    public GameObject currentScreen;
    public GameObject winScreen;
    public GameObject loseScreen;

    public void RestartGame()
    {
        // 重設 Timer
        if (timer != null)
        {
            timer.ResetTimer();
        }

        // 關閉結局畫面
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }

        if (loseScreen != null)
        {
            loseScreen.SetActive(false);
        }

        // 回到遊戲畫面
        if (currentScreen != null)
        {
            currentScreen.SetActive(true);
        }

        Debug.Log("RESTART COMPLETE");
    }
}