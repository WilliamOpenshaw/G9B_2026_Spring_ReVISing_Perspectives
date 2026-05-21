using UnityEngine;
using TMPro;

public class LaundryBabyManager : MonoBehaviour
{
    [Header("Game State")]
    public float matchTimer = 90f;
    public bool isGameActive = true;

    [Header("Room Panels")]
    public GameObject laundryPanel;
    public GameObject livingRoomPanel;     // Maze 1: Laundry -> Nursery
    public GameObject livingRoomBackPanel; // Maze 2: Nursery -> Laundry
    public GameObject nurseryPanel;

    [Header("Laundry Tracker")]
    public int totalClothesToWash = 50;
    private int clothesRemaining;
    public LaundrySortingGame laundrySortingScript; // Reference to the sorting game

    void Start()
    {
        clothesRemaining = totalClothesToWash;
        SwitchRoom("Laundry");
    }

    void Update()
    {
        if (!isGameActive) return;
        HandleMatchTimer();
    }

    void HandleMatchTimer()
    {
        matchTimer -= Time.deltaTime;
        if (matchTimer <= 0)
        {
            matchTimer = 0;
            LoseGame("Time ran out!");
        }
    }

    public void SwitchRoom(string roomName)
    {
        // 1. FORCE RESET BOTH PLAYERS INSTANTLY BEFORE TURNING PANELS ON/OFF
        ResetAllMazePlayers();

        // 2. Turn off all panels to prevent overlaps
        laundryPanel.SetActive(false);
        livingRoomPanel.SetActive(false);
        livingRoomBackPanel.SetActive(false);
        nurseryPanel.SetActive(false);

        // 3. Turn on the correct room
        if (roomName == "Laundry") laundryPanel.SetActive(true);
        if (roomName == "Nursery") nurseryPanel.SetActive(true);
        if (roomName == "LivingRoom") livingRoomPanel.SetActive(true);
        if (roomName == "LivingRoomBack") livingRoomBackPanel.SetActive(true);
    }

    // THE DEFINITION: This searches your panels and forces both players back to their start points
    private void ResetAllMazePlayers()
    {
        // Find the player in Maze 1 (Forward) and reset them
        if (livingRoomPanel != null && livingRoomPanel.activeInHierarchy)
        {
            MazePlayerController player1 = livingRoomPanel.GetComponentInChildren<MazePlayerController>();
            if (player1 != null) player1.ResetToStart();
        }

        // Find the player in Maze 2 (Backward) and reset them if the panel is active
        if (livingRoomBackPanel != null && livingRoomBackPanel.activeInHierarchy)
        {
            MazePlayerController player2 = livingRoomBackPanel.GetComponentInChildren<MazePlayerController>();
            if (player2 != null) player2.ResetToStart();
        }
    }

    public void CompleteMazeToNursery() { SwitchRoom("Nursery"); }
    public void CompleteMazeToLaundry() { SwitchRoom("Laundry"); }

    public void LoseGame(string reason)
    {
        isGameActive = false;
        Debug.Log($"GAME OVER: {reason}");
    }
}