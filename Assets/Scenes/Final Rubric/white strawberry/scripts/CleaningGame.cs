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
    private Vector2 presetClothPos;          // Stores original cloth position

    [Header("Phase 2: Trash Collection")]
    public RectTransform trashCanUI;         // The Trash Can UI Image slot
    public List<GameObject> trashItems;      // The list that holds your trash objects
    private int trashRemaining;

    [Header("Phase 3: Vacuuming")]
    public RectTransform vacuumUI;           // NEW SLOT: Drag your vacuum tool RectTransform here!
    public List<GameObject> dustBunnies;     // Drag all your dust objects here later
    private Vector2 presetVacuumPos;         // Stores original vacuum position

    private Dictionary<GameObject, float> dirtScrubTimers = new Dictionary<GameObject, float>();
    private float scrubRequiredTime = 1.5f; // Player must scrub a spot for 1.5 seconds to clean it

    // MULTI-DAY LOOP BACKUP STORAGE
    private List<GameObject> originalDirtSpots;
    private List<GameObject> originalTrashItems;
    private List<GameObject> originalDustBunnies;

    // DYNAMIC POSITION PRESET DICTIONARIES
    private Dictionary<GameObject, Vector2> initialObjectPositions = new Dictionary<GameObject, Vector2>();

    void OnEnable()
    {
        // 1. Snapshot setup: Save lists the absolute FIRST time the game ever boots up
        if (originalDirtSpots == null) originalDirtSpots = new List<GameObject>(dirtSpots);
        if (originalTrashItems == null) originalTrashItems = new List<GameObject>(trashItems);
        if (originalDustBunnies == null) originalDustBunnies = new List<GameObject>(dustBunnies);

        // Save preset tools positions before they move
        if (clothUI != null && presetClothPos == Vector2.zero) presetClothPos = clothUI.anchoredPosition;
        if (vacuumUI != null && presetVacuumPos == Vector2.zero) presetVacuumPos = vacuumUI.anchoredPosition;

        // Save starting layout positions for every individual trash piece, dirt spot, and dust bunny
        SaveInitialPositionsForList(originalDirtSpots);
        SaveInitialPositionsForList(originalTrashItems);
        SaveInitialPositionsForList(originalDustBunnies);

        // 2. Clear tracking references and restore master lists
        dirtSpots = new List<GameObject>(originalDirtSpots);
        trashItems = new List<GameObject>(originalTrashItems);
        dustBunnies = new List<GameObject>(originalDustBunnies);

        // 3. Forcefully wake up items, reset visibility, and snap them back to their exact presets!
        ResetAndPositionItems(dirtSpots, true);
        ResetAndPositionItems(trashItems, false);
        ResetAndPositionItems(dustBunnies, false);

        // 4. Snap cleaning tools back to their starting spots
        if (clothUI != null) clothUI.anchoredPosition = presetClothPos;
        if (vacuumUI != null) vacuumUI.anchoredPosition = presetVacuumPos;

        // 5. Reset clock counters and system values back to defaults
        timeRemaining = 60f; 
        isGameActive = true;
        dirtRemaining = dirtSpots.Count;
        trashRemaining = trashItems.Count;
        dirtScrubTimers.Clear();

        // 6. Turn off old end-screens and drop back into Phase 1 (Table Wipe)
        if (tableWipePanel != null) tableWipePanel.SetActive(true);
        if (trashCollectPanel != null) trashCollectPanel.SetActive(false);
        if (vacuumPanel != null) vacuumPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (failPanel != null) failPanel.SetActive(false);

        Debug.Log("Cleaning Game has successfully restored all items, positions, and clocks for the new day!");
    }

    // Helper function to memorize where items belong
    void SaveInitialPositionsForList(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            if (obj != null && !initialObjectPositions.ContainsKey(obj))
            {
                RectTransform rt = obj.GetComponent<RectTransform>();
                if (rt != null)
                {
                    initialObjectPositions[obj] = rt.anchoredPosition;
                }
            }
        }
    }

    // Helper function to wake up items and force them back to their presets
    void ResetAndPositionItems(List<GameObject> list, bool isDirtPhase)
    {
        foreach (GameObject obj in list)
        {
            if (obj != null)
            {
                obj.SetActive(true);

                // Snap the rect back to its exact saved layout coordinates
                RectTransform rt = obj.GetComponent<RectTransform>();
                if (rt != null && initialObjectPositions.ContainsKey(obj))
                {
                    rt.anchoredPosition = initialObjectPositions[obj];
                }

                // Restore alpha opacity if it was faded out during the scrubbing phase
                if (isDirtPhase)
                {
                    Image img = obj.GetComponent<Image>();
                    if (img != null) { Color c = img.color; c.a = 1f; img.color = c; }
                }
            }
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        HandleGlobalTimer();

        if (tableWipePanel.activeSelf)
        {
            HandleWipingLogic();
        }
    }

    void HandleGlobalTimer()
    {
        timeRemaining -= Time.deltaTime;
        
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"Time Left:   <color=red>{minutes}:{seconds:D2}</color>";

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            isGameActive = false;
            LoseGame();
        }
    }

    #region PHASE 1: WIPING CODE
    void HandleWipingLogic()
    {
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

            CheckDirtCollision();
        }
    }

    void CheckDirtCollision()
    {
        for (int i = dirtSpots.Count - 1; i >= 0; i--)
        {
            GameObject currentDirt = dirtSpots[i];
            if (currentDirt == null) continue;

            float distance = Vector2.Distance(clothUI.anchoredPosition, currentDirt.GetComponent<RectTransform>().anchoredPosition);
            
            if (distance < 50f) 
            {
                if (!dirtScrubTimers.ContainsKey(currentDirt))
                {
                    dirtScrubTimers[currentDirt] = 0f;
                }

                dirtScrubTimers[currentDirt] += Time.deltaTime;

                Image dirtImage = currentDirt.GetComponent<Image>();
                if (dirtImage != null)
                {
                    float progress = dirtScrubTimers[currentDirt] / scrubRequiredTime;
                    Color c = dirtImage.color;
                    c.a = 1f - progress; 
                    dirtImage.color = c;
                }

                if (dirtScrubTimers[currentDirt] >= scrubRequiredTime)
                {
                    if (cleanEffectPrefab != null)
                    {
                        GameObject effect = Instantiate(cleanEffectPrefab, tableWipePanel.transform);
                        effect.GetComponent<RectTransform>().anchoredPosition = currentDirt.GetComponent<RectTransform>().anchoredPosition;
                        Destroy(effect, 1f); 
                    }

                    dirtScrubTimers.Remove(currentDirt);
                    currentDirt.SetActive(false); // Hide it
                    
                    dirtSpots.RemoveAt(i);
                    dirtRemaining--;

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
            // 1. Hide it immediately so it vanishes from the player's view
            droppedTrash.SetActive(false); 
            
            // 2. Count how many trash items are still active on screen right now
            int activeTrashCount = 0;
            foreach (GameObject trash in trashItems)
            {
                if (trash != null && trash.activeSelf)
                {
                    activeTrashCount++;
                }
            }

            Debug.Log($"Trash thrown away! Items still left in the room: {activeTrashCount}");

            // 3. If there is no active trash left visible, automatically advance!
            if (activeTrashCount <= 0)
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
                    dust.SetActive(false); // Hide it

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
        isGameActive = false; 
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
        isGameActive = false;
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