using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class BabyBalanceGame : MonoBehaviour
{
    public LaundryBabyManager gameManager;
    public GameObject balanceGamePanel; // <--- Drag your new 'BalanceGameUI' object here!
    
    [Header("UI Elements")]
    public Slider balanceSlider; 
    public RectTransform goodZoneBarRect; 
    public GameObject warningFlashUI;
    public TextMeshProUGUI timerText;

    [Header("Settings")]
    public float tamingTimeLimit = 7f;
    public float playerPushSpeed = 0.6f;  
    public float maxCryTime = 2f;

    [Header("Green Zone Settings")]
    public float zoneMoveSpeed = 0.3f;    
    public float zoneWidth = 0.15f;       

    private float currentZoneCenter = 0.5f;
    private float zoneTarget = 0.5f;
    private float currentArrowVelocity = 0f; 

    private float currentTimer;
    private float outOfZoneTimer = 0f;
    private bool isGameActive = false;

    // 1. Add this variable near the top of your BabyBalanceGame script with your other flags:
    private bool isGameRunning = false;

    // 2. Change your StartTamingGame() function to look like this:
    public void StartTamingGame()
    {
        balanceGamePanel.SetActive(true);
        warningFlashUI.SetActive(false);
        isGameRunning = true; // Now the game officially starts!
        isGameActive = true; // Actually run the game logic!
        
        // Reset your taming timer and slider variables back to defaults here
        currentTimer = tamingTimeLimit;
        balanceSlider.value = 0.5f; // Center the slider
        outOfZoneTimer = 0f;
        currentZoneCenter = 0.5f;
        PickNewZoneTarget();
    }

    // 3. Change your ShowEmptyRoom() function to look like this:
    public void ShowEmptyRoom()
    {
        if (balanceGamePanel != null)
        {
            balanceGamePanel.SetActive(false);
        }
        if (warningFlashUI != null)
        {
            warningFlashUI.SetActive(false);
        }
        isGameRunning = false; // Completely stops the game from ticking down!
        isGameActive = false; // Stop game logic too
    }

    // 4. Go to your Update() function in BabyBalanceGame and add this line at the VERY TOP:
  

    void OnEnable() 
    {
        // Left blank on purpose! The Manager script will call the functions below instead.
    }

    

    void Update()
    {
        if (!isGameRunning) return;
        if (!isGameActive) return;

        currentTimer -= Time.deltaTime;
        timerText.text = "Taming Baby: " + currentTimer.ToString("F1") + "s";

        if (currentTimer <= 0)
        {
            isGameActive = false;
            gameManager.CompleteBabyTameSuccess();
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentArrowVelocity = -playerPushSpeed; 
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentArrowVelocity = playerPushSpeed;  
        }

        balanceSlider.value += currentArrowVelocity * Time.deltaTime;

        currentZoneCenter = Mathf.MoveTowards(currentZoneCenter, zoneTarget, zoneMoveSpeed * Time.deltaTime);
        if (Mathf.Abs(currentZoneCenter - zoneTarget) < 0.05f)
        {
            PickNewZoneTarget(); 
        }

        float halfWidth = zoneWidth / 2f;
        float goodZoneMin = currentZoneCenter - halfWidth;
        float goodZoneMax = currentZoneCenter + halfWidth;

        if (goodZoneBarRect != null)
        {
            goodZoneBarRect.anchorMin = new Vector2(goodZoneMin, 0f);
            goodZoneBarRect.anchorMax = new Vector2(goodZoneMax, 1f);
            goodZoneBarRect.offsetMin = Vector2.zero; 
            goodZoneBarRect.offsetMax = Vector2.zero;
        }

        if (balanceSlider.value >= goodZoneMin && balanceSlider.value <= goodZoneMax)
        {
            outOfZoneTimer = 0f;
            warningFlashUI.SetActive(false); 
        }
        else
        {
            outOfZoneTimer += Time.deltaTime;
            bool isFlashOn = Mathf.PingPong(Time.time * 6f, 1f) > 0.5f;
            warningFlashUI.SetActive(isFlashOn);

            if (outOfZoneTimer >= maxCryTime) 
            {
                isGameActive = false;
                gameManager.TriggerBabyCryGameOver();
            }
        }
    }

    void PickNewZoneTarget()
    {
        float limit = 1f - (zoneWidth / 2f);
        zoneTarget = Random.Range(zoneWidth / 2f, limit);
    }
}