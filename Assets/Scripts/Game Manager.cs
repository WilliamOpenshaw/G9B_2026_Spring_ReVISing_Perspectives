using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int foodCount = 15;

    public GameObject winScreen;
    public GameObject loseScreen;

    private void Awake()
    {
        Instance = this;
    }

    public void EatFood()
    {
        foodCount--;
        Debug.Log("Food Left = " + foodCount);
    }

    public void WinGame()
    {
        winScreen.SetActive(true);
    }

    public void LoseGame()
    {
        loseScreen.SetActive(true);
    }
}