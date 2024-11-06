using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion;
    [SerializeField] float destructionTime = 2f;
    [SerializeField] float explosionEffectTime = 0.1f;
    MeshRenderer meshRenderer;
    Collider sphereCollider;
    Rigidbody rgbody;




    // Start is called before the first frame update
    void Start()
    {

        meshRenderer = GetComponent<MeshRenderer>();
        sphereCollider = GetComponent<Collider>();
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
            Invoke(nameof(CollisionEffect), explosionEffectTime);
            Invoke(nameof(SelfDestruct), destructionTime);
            // SelfDestruct();
        }
    }
    void CollisionEffect()
    {
        explosion.Play();
        Destroy(rgbody);
        Destroy(meshRenderer);
        Destroy(sphereCollider);
    }

    void SelfDestruct()
    {
        Destroy(transform.gameObject);
    }
}
