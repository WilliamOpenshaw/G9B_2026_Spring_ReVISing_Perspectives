using UnityEngine;
using UnityEngine.UI;

public class mazeFoodCollider : MonoBehaviour
{
    public mazeGameManager gameManager;

    private Image foodImage;
    private Collider2D foodCollider;

    private void Start()
    {
        foodImage = GetComponent<Image>();
        foodCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.EatFood();

            if (foodImage != null)
                foodImage.enabled = false;

            if (foodCollider != null)
                foodCollider.enabled = false;
        }
    }
}