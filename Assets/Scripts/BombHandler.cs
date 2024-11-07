using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHandler : MonoBehaviour
{

    [SerializeField] ParticleSystem explosion;
    [SerializeField] float explosionDelay;
    [SerializeField] float destructionDelay;

    MeshRenderer meshRenderer;
    Collider boxCollider;
    Rigidbody rgbody;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<Collider>();
        rgbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Untagged"))
        {

            Invoke(nameof(ExplosionEffect), explosionDelay);
            Invoke(nameof(SelfDestruct), destructionDelay);
        }

    }
    void ExplosionEffect()
    {
        explosion.Play();
        Destroy(rgbody);
        Destroy(meshRenderer);
        Destroy(boxCollider);
    }
    void SelfDestruct()
    {
        Destroy(transform.gameObject);
    }
}
