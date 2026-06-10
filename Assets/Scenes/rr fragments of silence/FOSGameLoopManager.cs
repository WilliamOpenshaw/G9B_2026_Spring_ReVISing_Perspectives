using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    // Call this from your final win scene's button to restart everything flawlessly
    public void LoopEntireGame()
    {
        // Reloads the currently open scene from scratch, wiping all saved tracking states
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}