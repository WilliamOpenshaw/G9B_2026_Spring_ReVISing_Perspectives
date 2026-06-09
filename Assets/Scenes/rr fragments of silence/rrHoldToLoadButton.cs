using UnityEngine;
using UnityEngine.UI;

public class RrHoldToLoadButton : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider loadingSlider;

    [Header("Screen Swapping")]
    [Tooltip("The current UI screen group to turn off.")]
    [SerializeField] private GameObject currentScreen;

    [Tooltip("The next UI screen group to turn on.")]
    [SerializeField] private GameObject nextScreen;

    [Header("Settings")]
    [SerializeField] private float loadDuration = 2.0f;
    [SerializeField] private float drainSpeedMultiplier = 2.0f;

    private bool isPointerDown = false;
    private float currentProgress = 0f;
    private bool isActionTriggered = false;

    void Start()
    {
        if (loadingSlider != null) loadingSlider.value = 0f;
    }

    void Update()
    {
        if (isActionTriggered) return;

        if (isPointerDown)
        {
            currentProgress += Time.deltaTime / loadDuration;
            if (currentProgress >= 1f)
            {
                currentProgress = 1f;
                TriggerLoadingAction();
            }
        }
        else
        {
            currentProgress -= Time.deltaTime * drainSpeedMultiplier;
            if (currentProgress < 0f) currentProgress = 0f;
        }

        if (loadingSlider != null) loadingSlider.value = currentProgress;
    }

    public void OnPointerDown()
    {
        if (!isActionTriggered) isPointerDown = true;
    }

    public void OnPointerUp()
    {
        isPointerDown = false;
    }

    private void TriggerLoadingAction()
    {
        isActionTriggered = true;
        isPointerDown = false;

        // Hide the current group and show the next group
        if (currentScreen != null) currentScreen.SetActive(false);
        if (nextScreen != null) nextScreen.SetActive(true);

        Debug.Log("Swapped screens successfully!");
    }

    public void ResetButton()
    {
        isActionTriggered = false;
        isPointerDown = false;
        currentProgress = 0f;
        if (loadingSlider != null) loadingSlider.value = 0f;
    }
}