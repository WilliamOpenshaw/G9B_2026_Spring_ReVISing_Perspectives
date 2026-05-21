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
    public GameObject winPanel;
    public GameObject failPanel;

    [Header("Global Timer")]
    public TextMeshProUGUI timerText;
    [Header("Timer Settings")]
    public float timeRemaining = 60f; // 60 seconds to clean the whole room!
    public bool isGameActive = true;  // Keeps track of whether the game is running

    [Header("Phase 1: Table Wiping")]
    public GameObject cleanEffectPrefab;
    public RectTransform clothUI;            // Drag your cloth image here
    public List<GameObject> dirtSpots;       // A list of all dirt objects on the table
    private int dirtRemaining;

    [Header("Phase 2: Trash Collection")]
    public RectTransform trashCanUI;         // The Trash Can UI Image slot
    public List<GameObject> trashItems;      // The list that holds your trash objects
    private int trashRemaining;

    [Header("Phase 3: Vacuuming")]
    public List<GameObject> dustBunnies;     // Drag all your dust objects here later

    private Dictionary<GameObject, float> dirtScrubTimers = new Dictionary<GameObject, float>();
    private float scrubRequiredTime = 1.5f; // Player must scrub a spot for 1.5 seconds to clean it

    void Start()
    {
        timeRemaining = 60f;
        dirtRemaining = dirtSpots.Count;
        trashRemaining = trashItems.Count;

        // Set up initial panel states
        tableWipePanel.SetActive(true);
        trashCollectPanel.SetActive(false);
        vacuumPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (failPanel != null) failPanel.SetActive(false);
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
        timeRemaining -= Time.deltaTime;
        
        // Formats the timer into a clean 0:00 style
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"Time Left: {minutes}:{seconds:D2}";

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            isGameActive = false;
            Debug.Log("Out of time! The living room is still messy.");
            
            // HOOKED UP: This now instantly calls your fail popup screen!
            LoseGame();
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
                    if (cleanEffectPrefab != null)
                    {
                        // Create the sparkling effect exactly where the dirt was!
                        GameObject effect = Instantiate(cleanEffectPrefab, tableWipePanel.transform);
                        effect.GetComponent<RectTransform>().anchoredPosition = currentDirt.GetComponent<RectTransform>().anchoredPosition;
                        Destroy(effect, 1f); 
                    }

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
        trashCollectPanel.SetActive(true); 
        trashRemaining = trashItems.Count; 
    }
    #endregion

    #region PHASE 2: TRASH CODE
    public void CheckTrashDrop(GameObject droppedTrash)
    {
        float distance = Vector2.Distance(droppedTrash.GetComponent<RectTransform>().anchoredPosition, trashCanUI.anchoredPosition);

        if (distance < 80f) 
        {
            trashItems.Remove(droppedTrash);
            Destroy(droppedTrash);
            trashRemaining--;
            Debug.Log($"Trash thrown away! Remaining: {trashRemaining}");

            if (trashRemaining <= 0)
            {
                SwitchToVacuumPhase();
            }
        }
    }

    void SwitchToVacuumPhase()
    {
        Debug.Log("Room is tidy! Time to vacuum under the sofa.");
        trashCollectPanel.SetActive(false);
        vacuumPanel.SetActive(true); 
    }
    #endregion

    #region PHASE 3: VACUUM CODE
    public void CheckVacuumCollision(RectTransform vacuumRect)
    {
        for (int i = dustBunnies.Count - 1; i >= 0; i--)
        {
            GameObject dust = dustBunnies[i];
            if (dust != null)
            {
                RectTransform dustRect = dust.GetComponent<RectTransform>();
                float distance = Vector2.Distance(vacuumRect.anchoredPosition, dustRect.anchoredPosition);

                if (distance < 50f)
                {
                    dustBunnies.RemoveAt(i);
                    Destroy(dust);
                    Debug.Log($"Sucked up dust! Remaining: {dustBunnies.Count}");

                    if (dustBunnies.Count <= 0)
                    {
                        WinGame();
                    }
                }
            }
        }
    }

    void WinGame()
    {
        Debug.Log("CONGRATULATIONS! The entire room is completely spotless!");
        isGameActive = false; // Freeze the clock on win!
        vacuumPanel.SetActive(false);
        
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }
    #endregion

    #region FAIL SCREEN CODE
    void LoseGame()
    {
        Debug.Log("Game Over! You ran out of time.");
        isGameActive = false;

        // Hide active cleaning graphics
        tableWipePanel.SetActive(false);
        trashCollectPanel.SetActive(false);
        vacuumPanel.SetActive(false);

        if (failPanel != null)
        {
            failPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    #endregion
}