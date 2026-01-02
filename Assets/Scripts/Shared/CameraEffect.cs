using UnityEngine;
using CartoonFX;

public class CameraEffect : MonoBehaviour
{
    [Header("Camera Shake Settings")]
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] Vector3 shakeStrength = new Vector3(0.1f, 0.1f, 0.1f);
    private CFXR_Effect.CameraShake cameraShake;
    private float cameraShakeTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize camera shake
        InitializeCameraShake();
    }

    public void ShakeCamera()
    {
        if (cameraShake != null)
        {
            // Reset time and start shake
            cameraShakeTime = 0f;
            cameraShake.StartShake();
        }
    }

    void Update()
    {
        // Update camera shake if it's active
        if (cameraShake != null && cameraShake.isShaking)
        {
            cameraShakeTime += Time.deltaTime;
            cameraShake.animate(cameraShakeTime);
        }
    }
    void InitializeCameraShake()
    {
        cameraShake = new CFXR_Effect.CameraShake();
        cameraShake.enabled = true;
        cameraShake.useMainCamera = true;
        cameraShake.duration = shakeDuration;
        cameraShake.shakeStrength = shakeStrength;
        cameraShake.shakeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        // Fetch cameras to register them
        cameraShake.fetchCameras();
    }
}
