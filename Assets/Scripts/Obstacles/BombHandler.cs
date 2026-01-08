using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class BombHandler : MonoBehaviour
{

    [SerializeField] ParticleSystem explosion;
    [SerializeField] float explosionDelay;
    [SerializeField] float destructionDelay;
    [SerializeField] GameDataSave gameDataSave;


    private MeshRenderer meshRenderer;
    private Collider boxCollider;
    private Rigidbody rgbody;

    private AudioSource audioSource;
    public ObjectPool<GameObject> pool;




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
        audioSource = GetComponent<AudioSource>();

    }






    private void OnCollisionEnter(Collision other)
    {

        if (!other.gameObject.CompareTag("BombLayer"))
        {

            // // Shake camera if gameObject has tag of "Letter Cube"
            // if (other.gameObject.CompareTag("Letter Cube"))
            // {
            //     //.Log("BombHandler: Letter Cube detected, triggering camera shake!");
            //     ShakeCamera();
            // }

            if (!gameDataSave.MuteAllAudio)
                audioSource.Play();
                
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
