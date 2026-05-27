using UnityEngine;
using UnityEngine.UI;

public class wt_movement_blue : MonoBehaviour
{
    // four gameobjects for ui images of direction of character movement
    public GameObject upImage;
    public GameObject downImage;
    public GameObject leftImage;
    public GameObject rightImage;
    public float speed = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upImage.SetActive(true);
        downImage.SetActive(false);
        leftImage.SetActive(false);
        rightImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // move character ui image up with rect transform anchor in canvas when w key is pressed
        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<RectTransform>().anchoredPosition += new Vector2(0, speed);
            upImage.SetActive(true);
            downImage.SetActive(false);
            leftImage.SetActive(false);
            rightImage.SetActive(false);
        }
        // move character ui imnage down with rect transform anchor in canvas when s key is pressed
        if (Input.GetKey(KeyCode.S))        {
            GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -speed);
            upImage.SetActive(false);
            downImage.SetActive(true);
            leftImage.SetActive(false);
            rightImage.SetActive(false);
        }
         // move character ui image left with rect transform anchor in canvas when a key is pressed
        if (Input.GetKey(KeyCode.A))        {
            GetComponent<RectTransform>().anchoredPosition += new Vector2(-speed, 0);
            upImage.SetActive(false);
            downImage.SetActive(false);
            leftImage.SetActive(true);
            rightImage.SetActive(false);
        }
         // move character ui image right with rect transform anchor in canvas when d key is pressed
        if (Input.GetKey(KeyCode.D))        {
            GetComponent<RectTransform>().anchoredPosition += new Vector2(speed, 0);
            upImage.SetActive(false);
            downImage.SetActive(false);
            leftImage.SetActive(false);
            rightImage.SetActive(true);
        }

    
    }
}
// 