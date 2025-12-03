using UnityEngine;

public class MobilePerformanceManager : MonoBehaviour
{
    public static MobilePerformanceManager Instance { get; private set; }

    [Range(30, 120)]
    public int targetFPS = 60;

    [Range(0.5f, 1f)]
    public float renderScaleHigh = 1.0f;
    [Range(0.5f, 1f)]
    public float renderScaleLow = 0.75f;

    private void Awake()
    {
        // Singleton pattern to persist across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Apply initial settings
        ApplyPerformanceSettings();
    }

    private void ApplyPerformanceSettings()
    {
        // Disable VSync for consistent Application.targetFrameRate control
        QualitySettings.vSyncCount = 0;

        // Set desired framerate
        Application.targetFrameRate = targetFPS;

        // Detect if the device is weak or strong
        bool isWeakDevice = DetectWeakDevice();

        // Apply render scale
        if (isWeakDevice)
        {
            Debug.Log("Weak device detected → Applying lower render scale.");
            ScalableBufferManager.ResizeBuffers(renderScaleLow, renderScaleLow);
        }
        else
        {
            // Debug.Log("Strong device detected → Using full render scale.");
            ScalableBufferManager.ResizeBuffers(renderScaleHigh, renderScaleHigh);
        }

        // Apply other quality settings
        if (isWeakDevice)
        {
            QualitySettings.shadowDistance = 15f;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            QualitySettings.pixelLightCount = 1;
        }
        else
        {
            QualitySettings.shadowDistance = 50f;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
            QualitySettings.pixelLightCount = 4;
        }
    }

    private bool DetectWeakDevice()
    {

        string gpu = SystemInfo.graphicsDeviceName.ToLower();
        int ram = SystemInfo.systemMemorySize;

        // Debug.Log($"Device: {SystemInfo.deviceModel}, GPU: {SystemInfo.graphicsDeviceName}, RAM: {ram}MB");

        if (ram < 5000) return true; // Less than 5 GB RAM → weak
        if (gpu.Contains("mali") || gpu.Contains("adreno 6")) return true; // Mid-tier GPU families

        return false; // Otherwise assume strong device
    }
}
