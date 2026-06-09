using UnityEngine;

public class crossingMoveCharacter : MonoBehaviour
{
    public float speed = 500f; // Speed of the character
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if press w or up arrow then this gameobjects 2d rigidbody gets moved up
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, speed);
        }
         //if press s or down arrow then this gameobjects 2d rigidbody gets moved down
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -speed);
        }
         //if press a or left arrow then this gameobjects 2d rigidbody gets moved left
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-speed, 0);
        }
         //if press d or right arrow then this gameobjects 2d rigidbody gets moved right
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(speed, 0);
        }
         //if no key is pressed then this gameobjects 2d rigidbody gets stopped
        else
        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
        }


    }
}
