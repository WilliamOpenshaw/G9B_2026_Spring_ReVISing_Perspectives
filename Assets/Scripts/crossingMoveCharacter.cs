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
        //if press w then this gameobjects 2d rigidbody gets moved up
        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, speed);
        }
         //if press s then this gameobjects 2d rigidbody gets moved down
        else if (Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -speed);
        }
         //if press a then this gameobjects 2d rigidbody gets moved left
        else if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-speed, 0);
        }
         //if press d then this gameobjects 2d rigidbody gets moved right
        else if (Input.GetKey(KeyCode.D))
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
