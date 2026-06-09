using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;

    void Awake()
    {
        // 1. Turn off VSync so Unity relies on targetFrameRate
        QualitySettings.vSyncCount = 0;

        // 2. Set the custom frame rate cap
        Application.targetFrameRate = targetFPS;
    }
}