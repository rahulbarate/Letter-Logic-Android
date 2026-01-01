using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using CartoonFX;

public class BombHandler : MonoBehaviour
{

    [SerializeField] ParticleSystem explosion;
    [SerializeField] float explosionDelay;
    [SerializeField] float destructionDelay;
    [Header("Camera Shake Settings")]
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] Vector3 shakeStrength = new Vector3(0.1f, 0.1f, 0.1f);

    private MeshRenderer meshRenderer;
    private Collider boxCollider;
    private Rigidbody rgbody;
    public ObjectPool<GameObject> pool;

    private CFXR_Effect.CameraShake cameraShake;
    private float cameraShakeTime = 0f;

    public void ResetForPool()
    {
        // Initialize components if they haven't been initialized yet
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
        if (boxCollider == null)
            boxCollider = GetComponent<Collider>();
        if (rgbody == null)
            rgbody = GetComponent<Rigidbody>();

        // Reset component states
        if (meshRenderer != null)
            meshRenderer.enabled = true;
        if (boxCollider != null)
            boxCollider.enabled = true;
        if (rgbody != null)
            rgbody.isKinematic = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //.Log("BombHandler: Start() called");

        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<Collider>();
        rgbody = GetComponent<Rigidbody>();

        // Initialize camera shake
        InitializeCameraShake();
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

    void ShakeCamera()
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


    private void OnCollisionEnter(Collision other)
    {
        
        if (!other.gameObject.CompareTag("BombLayer"))
        {

            // Shake camera if gameObject has tag of "Letter Cube"
            if (other.gameObject.CompareTag("Letter Cube"))
            {
                //.Log("BombHandler: Letter Cube detected, triggering camera shake!");
                ShakeCamera();
            }

            Invoke(nameof(ExplosionEffect), explosionDelay);
            Invoke(nameof(SelfDestruct), destructionDelay);

        }

    }
    void ExplosionEffect()
    {
        explosion.Play();
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        rgbody.isKinematic = true;
    }
    void SelfDestruct()
    {
        pool.Release(gameObject);
    }
}
