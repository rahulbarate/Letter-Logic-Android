using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion;
    [SerializeField] float destructionTime = 2f;
    [SerializeField] float explosionEffectTime = 0.1f;
    [SerializeField] GameDataSave gameDataSave;
    public MeshRenderer meshRenderer;
    public Collider sphereCollider;
    Rigidbody rgbody;
    AudioSource audioSource;
    public CannonHandler cannonHandler;




    // Start is called before the first frame update
    void Start()
    {

        meshRenderer = GetComponent<MeshRenderer>();
        sphereCollider = GetComponent<Collider>();
        rgbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }


    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Untagged"))
        {
            if (!gameDataSave.MuteAllAudio)
                audioSource.Play();
            // Debug.Log("Cannon hit " + other.gameObject.tag);
            Invoke(nameof(ExplosionEffect), explosionEffectTime);
            Invoke(nameof(SelfDestruct), destructionTime);
            // SelfDestruct();
        }
    }
    void ExplosionEffect()
    {
        explosion.Play();
        rgbody.isKinematic = true;
        meshRenderer.enabled = false;
        sphereCollider.enabled = false;
    }

    void SelfDestruct()
    {
        cannonHandler.ReturnToPool(transform.gameObject);
    }
}
