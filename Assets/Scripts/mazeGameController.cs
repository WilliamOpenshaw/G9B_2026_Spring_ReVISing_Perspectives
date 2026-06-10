using UnityEngine;

public class mazeGameController : MonoBehaviour
{
    public mazeGameManager gameManager;
    public RectTransform exitZone;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        CheckExitCollision();
    }

    void CheckExitCollision()
    {
        if (exitZone == null) return;

        Vector3[] playerCorners = new Vector3[4];
        rectTransform.GetWorldCorners(playerCorners);

        foreach (Vector3 corner in playerCorners)
        {
            Vector2 screenPoint =
                RectTransformUtility.WorldToScreenPoint(null, corner);

            if (RectTransformUtility.RectangleContainsScreenPoint(
                exitZone,
                screenPoint,
                null))
            {
                Debug.Log("Player reached the exit!");
                Debug.Log("Food Count = " + gameManager.foodCount);

                if (gameManager != null)
                {
                    if (gameManager.foodCount <= 0)
                    {
                        Debug.Log("WIN");
                        gameManager.WinGame();
                    }
                    else
                    {
                        Debug.Log("LOSE");
                        gameManager.LoseGame();
                    }
                }

                return;
            }
        }
    }
}