using UnityEngine;
using UnityEngine.UI; // Required for Image components
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Interaction Setup")]
    [SerializeField] private PuzzlePiece[] puzzlePieces;

    [Header("Screen Swapping Navigation")]
    [SerializeField] private GameObject puzzleWorkbenchScreen;
    [SerializeField] private GameObject gameplayChoiceScreen;

    private bool isFinishing = false;

    void Start()
    {
        if (puzzleWorkbenchScreen != null) puzzleWorkbenchScreen.SetActive(false);
        if (gameplayChoiceScreen != null) gameplayChoiceScreen.SetActive(false);
    }

    public void CheckPuzzleCompletion()
    {
        if (puzzlePieces == null || puzzlePieces.Length == 0 || isFinishing) return;

        foreach (PuzzlePiece piece in puzzlePieces)
        {
            if (piece == null || !piece.IsSnapped()) return; 
        }

        StartCoroutine(GoldenVictorySequence());
    }

    private IEnumerator GoldenVictorySequence()
    {
        isFinishing = true;

        // Rich golden color glow
        Color goldGlow = new Color(1f, 0.82f, 0.15f, 0.75f); 

        // Light up ONLY the guideline backdrops as UI Image shapes
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            if (piece != null)
            {
                Image guidelineRen = piece.GetGuidelineRenderer(); // Swapped to Image
                if (guidelineRen != null)
                {
                    guidelineRen.enabled = true; 
                    guidelineRen.color = goldGlow; 
                }
            }
        }

        yield return new WaitForSeconds(2.0f);

        if (puzzleWorkbenchScreen != null) puzzleWorkbenchScreen.SetActive(false);
        if (gameplayChoiceScreen != null) gameplayChoiceScreen.SetActive(true);
    }
}