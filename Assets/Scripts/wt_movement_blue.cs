using UnityEngine;
using UnityEngine.UI;

public class wt_movement_blue : MonoBehaviour
{

    public float speed = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move character ui image up with rect transform anchor in canvas when w key is pressed
        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<RectTransform>().anchoredPosition += new Vector2(0, speed);
        }
        // move character ui imnage down with rect transform anchor in canvas when s key is pressed
        if (Input.GetKey(KeyCode.S))        {
            GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -speed);
        }
         // move character ui image left with rect transform anchor in canvas when a key is pressed
        if (Input.GetKey(KeyCode.A))        {
            GetComponent<RectTransform>().anchoredPosition += new Vector2(-speed, 0);
        }
         // move character ui image right with rect transform anchor in canvas when d key is pressed
        if (Input.GetKey(KeyCode.D))        {
            GetComponent<RectTransform>().anchoredPosition += new Vector2(speed, 0);
        }

    
    }
}
// 