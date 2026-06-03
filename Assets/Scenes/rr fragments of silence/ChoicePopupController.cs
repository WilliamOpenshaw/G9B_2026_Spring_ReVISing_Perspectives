using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ChoicePopupController : MonoBehaviour
{
    [Header("Popup Text Components")]
    [SerializeField] private TextMeshProUGUI correctText;
    [SerializeField] private TextMeshProUGUI incorrectText;

    [Header("Timing Settings (Total: 1 Second)")]
    [SerializeField] private float visibleDuration = 0.3f; // Solid on screen for 0.3s
    [SerializeField] private float fadeOutDuration = 0.7f; // Quick fade out over 0.7s

    [Header("Heart Delay Settings")]
    [Tooltip("Connect your Heart Health game object here, then select FOSHealth -> loseHeart()")]
    [SerializeField] private UnityEvent onIncorrectSceneOpened;

    private Coroutine activeAnimRoutine;
    private static bool lastChoiceWasCorrect = true;

    // This runs automatically the exact millisecond the '1 letter answer' scene object turns on
    private void OnEnable()
    {
        if (lastChoiceWasCorrect)
        {
            if (correctText != null) PrepareAndStartSequence(correctText, incorrectText);
        }
        else
        {
            if (incorrectText != null) PrepareAndStartSequence(incorrectText, correctText);
            
            // This pulls the trigger on your separate heart script automatically!
            if (onIncorrectSceneOpened != null)
            {
                onIncorrectSceneOpened.Invoke();
            }
        }
    }

    public void DisplayCorrect()
    {
        lastChoiceWasCorrect = true;
    }

    public void DisplayIncorrect()
    {
        lastChoiceWasCorrect = false;
    }

    private void PrepareAndStartSequence(TextMeshProUGUI showText, TextMeshProUGUI hideText)
    {
        if (hideText != null) hideText.gameObject.SetActive(false);
        showText.gameObject.SetActive(true);

        if (activeAnimRoutine != null)
        {
            StopCoroutine(activeAnimRoutine);
        }

        activeAnimRoutine = StartCoroutine(SimpleFadeSequence(showText));
    }

    private IEnumerator SimpleFadeSequence(TextMeshProUGUI targetText)
    {
        targetText.transform.localScale = Vector3.one;
        Color defaultColor = targetText.color;
        targetText.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 1f);

        yield return new WaitForSeconds(visibleDuration);

        float timeSpent = 0f;
        while (timeSpent < fadeOutDuration)
        {
            timeSpent += Time.deltaTime;
            float alphaValue = Mathf.Lerp(1f, 0f, timeSpent / fadeOutDuration);
            targetText.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, alphaValue);
            yield return null;
        }

        targetText.gameObject.SetActive(false);
    }
}