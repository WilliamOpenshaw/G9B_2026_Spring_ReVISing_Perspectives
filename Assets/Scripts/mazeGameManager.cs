using UnityEngine;
using UnityEngine.UI;

public class mazeGameManager : MonoBehaviour
{
    public static mazeGameManager Instance;

    public int foodCount = 15;

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject mazeScreen;

    public GameObject[] foods;

    private void Awake()
    {
        Debug.Log("=================================");
        Debug.Log("Manager Awake");
        Debug.Log("Object Name = " + gameObject.name);
        Debug.Log("Instance ID = " + GetInstanceID());
        Debug.Log("=================================");

        Instance = this;

        foodCount = 15;

        if (winScreen != null)
            winScreen.SetActive(false);

        if (loseScreen != null)
            loseScreen.SetActive(false);

        if (mazeScreen != null)
            mazeScreen.SetActive(true);
    }

    public void EatFood()
    {
        foodCount--;

        if (foodCount < 0)
            foodCount = 0;

        Debug.Log(
            "Food Left = "
            + foodCount
            + " Manager = "
            + GetInstanceID()
        );
    }

    public void WinGame()
    {
        Debug.Log("WIN GAME");

        if (mazeScreen != null)
            mazeScreen.SetActive(false);

        if (winScreen != null)
            winScreen.SetActive(true);
    }

    public void LoseGame()
    {
        Debug.Log("LOSE GAME");

        if (mazeScreen != null)
            mazeScreen.SetActive(false);

        if (loseScreen != null)
            loseScreen.SetActive(true);
    }

    public void ResetGame()
    {
        Debug.Log("RESET GAME CALLED");
        Debug.Log("Manager ID = " + GetInstanceID());

        foodCount = 15;

        foreach (GameObject food in foods)
        {
            if (food != null)
            {
                Image img = food.GetComponent<Image>();
                Collider2D col = food.GetComponent<Collider2D>();

                if (img != null)
                    img.enabled = true;

                if (col != null)
                    col.enabled = true;

                Debug.Log("Reset Food: " + food.name);
            }
        }

        if (winScreen != null)
            winScreen.SetActive(false);

        if (loseScreen != null)
            loseScreen.SetActive(false);

        if (mazeScreen != null)
            mazeScreen.SetActive(true);

        Debug.Log("Maze Reset Complete");
    }
}