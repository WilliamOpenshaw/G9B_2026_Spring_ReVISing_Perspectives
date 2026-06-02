using UnityEngine;

public class crossingHitDetect : MonoBehaviour
{
    public GameObject thisScreen;
    public GameObject nextButton;


    //if player object collides with trigger of other gameobject, then enable a third gameobject
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nextButton.SetActive(true);
            //thisScreen.SetActive(false);
        }
    }
}
