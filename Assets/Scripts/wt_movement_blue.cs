using UnityEngine;
using UnityEngine.UI;

public class wt_movement_blue : MonoBehaviour
{
    // four gameobjects for ui images of direction of character movement
    public GameObject upImage;
    public GameObject downImage;
    public GameObject leftImage;
    public GameObject rightImage;
    public float speed = 1000f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upImage.SetActive(true);
        downImage.SetActive(false);
        leftImage.SetActive(false);
        rightImage.SetActive(false);
        speed = 1000f;
    }

    // Update is called once per frame
    void Update()
    {
        // move rigidbody of character when w key is pressed
        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, speed);
            upImage.SetActive(true);
            downImage.SetActive(false);
            leftImage.SetActive(false);
            rightImage.SetActive(false);
        }
        // move rigidbody of character down when s key is pressed
        if (Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -speed);
            upImage.SetActive(false);
            downImage.SetActive(true);
            leftImage.SetActive(false);
            rightImage.SetActive(false);
        }
         // move rigidbody of character left when a key is pressed
        if (Input.GetKey(KeyCode.A))        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-speed, 0);
            upImage.SetActive(false);
            downImage.SetActive(false);
            leftImage.SetActive(true);
            rightImage.SetActive(false);
        }
         // move rigidbody of character right when d key is pressed
        if (Input.GetKey(KeyCode.D))        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(speed, 0);
            upImage.SetActive(false);
            downImage.SetActive(false);
            leftImage.SetActive(false);
            rightImage.SetActive(true);
        }

    
    }
}
// 