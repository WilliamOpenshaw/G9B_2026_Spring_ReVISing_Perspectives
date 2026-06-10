using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Enum list stays perfect
    public enum GameMode { Baby, Easy, Hard, Impossible }
    public static GameMode CurrentMode = GameMode.Hard; // Defaults to Hard

    // --- NEW SECRET BUTTON FUNCTION ---
    public void SetDifficultyImpossible()
    {
        CurrentMode = GameMode.Impossible;
        Debug.Log("⚠️ SECRET IMPOSSIBLE MODE ACTIVATED VIA EASTER EGG! 💀");
        StartTheGame();
    }
    // ----------------------------------

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
        // Your scene loader or game-start code here!
    }
}