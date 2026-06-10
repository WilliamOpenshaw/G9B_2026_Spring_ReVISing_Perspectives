using UnityEngine;

public class MazePlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;

    public RectTransform startPosition;
    public RectTransform exitZone;
    public bool isGoingToNursery = false;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = Vector2.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            movement.x = -1;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            movement.x = 1;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            movement.y = 1;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            movement.y = -1;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

    public void StartMazeGame()
    {
        gameObject.SetActive(true);

        if (startPosition != null)
        {
            transform.position = startPosition.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            Debug.Log("Food Count At Goal = " + mazeGameManager.Instance.foodCount);

            mazeTimer2 timer = FindFirstObjectByType<mazeTimer2>();

            if (mazeGameManager.Instance.foodCount <= 0)
            {
                Debug.Log("WIN PATH");

                if (timer != null)
                {
                    timer.PlayerWon();
                }
            }
            else
            {
                Debug.Log("LOSE PATH");

                mazeGameManager.Instance.LoseGame();
            }
        }
    }
}