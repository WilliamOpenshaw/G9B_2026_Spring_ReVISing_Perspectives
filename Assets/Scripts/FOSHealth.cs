using System.Collections;
using UnityEngine;

public class FOSHealth : MonoBehaviour
{
    public int numberOfHearts = 3;
    public GameObject loseScreen;
    public GameObject timer;
    
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void loseHeart()
    {
        numberOfHearts -= 1;

        // Find which heart needs to blink based on the damage taken
        if (numberOfHearts == 2)
        {
            StartCoroutine(BlinkAndHideRoutine(heart3, false));
        }
        else if (numberOfHearts == 1)
        {
            StartCoroutine(BlinkAndHideRoutine(heart2, false));
        }
        else if (numberOfHearts <= 0)
        {
            // For the last heart, we pass 'true' so it triggers the lose screen after blinking
            StartCoroutine(BlinkAndHideRoutine(heart1, true));
        }
    }

    // This handles the custom 2-flash blinking animation smoothly over time
    private IEnumerator BlinkAndHideRoutine(GameObject targetHeart, bool triggerLoseScreen)
    {
        if (targetHeart != null)
        {
            float blinkDelay = 0.15f; // Speed of the blink sequence

            // Blink 1
            targetHeart.SetActive(false);
            yield return new WaitForSeconds(blinkDelay);
            targetHeart.SetActive(true);
            yield return new WaitForSeconds(blinkDelay);

            // Blink 2
            targetHeart.SetActive(false);
            yield return new WaitForSeconds(blinkDelay);
            targetHeart.SetActive(true);
            yield return new WaitForSeconds(blinkDelay);

            // Final disappear
            targetHeart.SetActive(false);
        }

        // If that was the final heart, wait an extra tiny moment and show the lose screen
        if (triggerLoseScreen)
        {
            yield return new WaitForSeconds(0.2f);
            if (timer != null) timer.SetActive(false);
            if (loseScreen != null) loseScreen.SetActive(true);
        }
    }

    // NEW LOOPING FUNCTION ADDED BELOW:
    public void ResetHearts()
    {
        numberOfHearts = 3; // Resets your score tracking counter
        
        // Instantly reactivates all your heart UI elements
        if (heart1 != null) heart1.SetActive(true);
        if (heart2 != null) heart2.SetActive(true);
        if (heart3 != null) heart3.SetActive(true);
    }
}