using UnityEngine;



public class crossingHitDetect : MonoBehaviour
{
    public GameObject thisScreen;
    public GameObject nextButton;

    public GameObject player;
    public Transform playerStartingPosition;

    //if player object collides with trigger of this gameobject, then enable next button
    public void OnEnable()
    {
        nextButton.SetActive(false);
        //reset player position and rotation to starting position
        player.transform.position = playerStartingPosition.position;
        player.transform.rotation = playerStartingPosition.rotation;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit detected");
        if (other.CompareTag("Player"))
        {
            Debug.Log("player hit detected");
            nextButton.SetActive(true);
            //thisScreen.SetActive(false);
        }
    }
}
