using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class useDoor : MonoBehaviour
{
    // ugui tmpro text variable
    public TextMeshProUGUI  doorpressText;
    

    public GameObject thisroom;

    public GameObject nextroom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorpressText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if collision detected wait for press e key to disable and enable next room gameobject
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if doorpress text is enabled then disable it and enable next room gameobject
                if (doorpressText.enabled == true)
                {
                    doorpressText.enabled = false;
                    // enable next room gameobject
                    nextroom.SetActive(true);
                    // disable this room gameobject
                    thisroom.SetActive(false);
                }
            }
    }

    // ontriggerenter2d instead


    void OnTriggerEnter2D(Collider2D collision) 
    { 
        if (collision.gameObject.CompareTag("Player")) 
        { 
            doorpressText.enabled = true;

        } 
    } 

    void OnTriggerExit2D(Collider2D collision) 
    { 
        if (collision.gameObject.CompareTag("Player")) 
        { 
            doorpressText.enabled = false;

        } 
    } 

}
