using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public enum GameMode { Baby, Easy, Hard }
    public static GameMode CurrentMode = GameMode.Hard; // Defaults to Hard

    public void SetDifficultyBaby() // (Easy Mode Button)
    {
        CurrentMode = GameMode.Baby;
        Debug.Log("Difficulty set to: EASY MODE 🍼");
        StartTheGame();
    }

    public void SetDifficultyEasy() // (Medium Mode Button)
    {
        CurrentMode = GameMode.Easy;
        Debug.Log("Difficulty set to: MEDIUM MODE 🟢");
        StartTheGame();
    }

    public void SetDifficultyHard() // (Hard Mode Button)
    {
        CurrentMode = GameMode.Hard;
        Debug.Log("Difficulty set to: HARD MODE 🔴");
        StartTheGame();
    }

    private void StartTheGame()
    {
        // Put whatever you use to clear the menu or load Day 1 here!
    }
}