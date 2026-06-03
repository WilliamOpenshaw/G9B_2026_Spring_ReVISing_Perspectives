using UnityEngine;

public class TriggerHeartLossOnEnable : MonoBehaviour
{
    [Header("Link your Heart Health object here")]
    public FOSHealth healthSystem;

    private void OnEnable()
    {
        // The exact frame this screen turns on, it docks a heart automatically!
        if (healthSystem != null)
        {
            healthSystem.loseHeart();
        }
    }
}