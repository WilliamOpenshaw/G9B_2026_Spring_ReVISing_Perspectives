using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CleaningGame : MonoBehaviour
{
    [Header("Game Phase Panels")]
    public GameObject tableWipePanel;
    public GameObject trashCollectPanel;
    public GameObject vacuumPanel;

    [Header("Global Timer")]
    public TextMeshProUGUI timerText;
    public float totalCleanTime = 60f; // 1 minute to clean everything!
    private float currentTimer;
    private bool isGameActive = true;

    [Header("Phase 1: Table Wiping")]
    public GameObject cleanEffectPrefab;
    public RectTransform clothUI;            // Drag your cloth image here
    public List<GameObject> dirtSpots;       // A list of all dirt objects on the table
    private int dirtRemaining;
    private bool isWiping = false;

    void Start()
    {
        currentTimer = totalCleanTime;
        dirtRemaining = dirtSpots.Count;

        // Set up initial panel states
        tableWipePanel.SetActive(true);
        trashCollectPanel.SetActive(false);
        vacuumPanel.SetActive(false);
    }

    void Update()
    {
        if (!isGameActive) return;

        // Ticking down the 1-minute clock
        HandleGlobalTimer();

        // If we are in Phase 1, handle the dragging/wiping logic
        if (tableWipePanel.activeSelf)
        {
            HandleWipingLogic();
        }
    }

    void HandleGlobalTimer()
    {
        currentTimer -= Time.deltaTime;
        
        // Formats the timer into a clean 0:00 style
        int minutes = Mathf.FloorToInt(currentTimer / 60);
        int seconds = Mathf.FloorToInt(currentTimer % 60);
        timerText.text = $"Time Left: {minutes}:{seconds:D2}";

        if (currentTimer <= 0)
        {
            isGameActive = false;
            Debug.Log("Out of time! The living room is still messy.");
            // We will hook up your Game Over popup panel here later!
        }
    }

    #region PHASE 1: WIPING CODE
    void HandleWipingLogic()
    {
        // Follow the mouse position smoothly while holding click
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tableWipePanel.GetComponent<RectTransform>(), 
                Input.mousePosition, 
                null, 
                out mousePos
            );
            clothUI.anchoredPosition = mousePos;

            // Check if our cloth is touching any dirt spots
            CheckDirtCollision();
        }
    }

    private float scrubRequiredTime = 1.5f; // Player must scrub a spot for 1.5 seconds to clean it
private Dictionary<GameObject, float> dirtScrubTimers = new Dictionary<GameObject, float>();

    void CheckDirtCollision()
    {
        // Loop backwards through the dirt spots
    for (int i = dirtSpots.Count - 1; i >= 0; i--)
    {
        GameObject currentDirt = dirtSpots[i];
        if (currentDirt == null) continue;

        // Calculate distance between cloth and this dirt spot
        float distance = Vector2.Distance(clothUI.anchoredPosition, currentDirt.GetComponent<RectTransform>().anchoredPosition);
        
        // Is the cloth actively touching the dirt?
        if (distance < 50f) 
        {
            // If this is the first time touching this dirt, start a timer for it
            if (!dirtScrubTimers.ContainsKey(currentDirt))
            {
                dirtScrubTimers[currentDirt] = 0f;
            }

            // Add time to this specific dirt's scrub timer
            dirtScrubTimers[currentDirt] += Time.deltaTime;

            // VISUAL EFFECT: Make the dirt slowly fade out the more you scrub it!
            Image dirtImage = currentDirt.GetComponent<Image>();
            if (dirtImage != null)
            {
                float progress = dirtScrubTimers[currentDirt] / scrubRequiredTime;
                Color c = dirtImage.color;
                c.a = 1f - progress; // Lower the visibility (alpha) based on progress
                dirtImage.color = c;
            }

            // If they have scrubbed long enough, completely delete the dirt!
            if (dirtScrubTimers[currentDirt] >= scrubRequiredTime)
            {
                // *** ADD THIS NEW BLOCK OF CODE HERE ***
                if (cleanEffectPrefab != null)
                {
                    // Create the sparkling effect exactly where the dirt was!
                    GameObject effect = Instantiate(cleanEffectPrefab, tableWipePanel.transform);
                    effect.GetComponent<RectTransform>().anchoredPosition = currentDirt.GetComponent<RectTransform>().anchoredPosition;
                    // Automatically delete the effect after 1 second so they don't clutter the screen
                    Destroy(effect, 1f); 
                }
                // *** END NEW BLOCK ***

                dirtScrubTimers.Remove(currentDirt);
                Destroy(currentDirt);
                dirtSpots.RemoveAt(i);
                dirtRemaining--;
                Debug.Log($"Scrubbed away! Dirt remaining: {dirtRemaining}");

                if (dirtRemaining <= 0)
                {
                    SwitchToTrashPhase();
                }
            }
        }
    }
    }

    void SwitchToTrashPhase()
    {
        Debug.Log("Table clean! Moving to Trash Collection.");
        tableWipePanel.SetActive(false);
        trashCollectPanel.SetActive(true); // Turns on your next panel automatically!
    }
    #endregion
}
