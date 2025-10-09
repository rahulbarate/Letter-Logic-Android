using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHandler : MonoBehaviour
{

    [SerializeField] GameObject cannonBallCopy;
    [SerializeField] GameObject cannonBallSpawnPoint;
    [SerializeField] float forceAmount = 500f;
    [SerializeField] int poolSize = 10;
    Queue<GameObject> cannonBallPool = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(cannonBallCopy);
            obj.SetActive(false);
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            cannonBallPool.Enqueue(obj);
        }
    }

    public void FireCannon()
    {
        if (cannonBallPool.Count > 0)
        {
            GameObject ball = cannonBallPool.Dequeue();
            ball.transform.position = cannonBallSpawnPoint.transform.position;
            ball.transform.rotation = Quaternion.identity;
            ball.transform.localScale = new Vector3(27f, 27f, 27f);
            ball.SetActive(true);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ball.GetComponent<CannonBallHandler>().cannonHandler = this;
            Vector3 localForce = new Vector3(0f, 0f, forceAmount);
            Vector3 worldForce = transform.TransformDirection(localForce);
            rb.AddForce(worldForce, ForceMode.Impulse);
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        CannonBallHandler handler = obj.GetComponent<CannonBallHandler>();
        handler.meshRenderer.enabled = true;
        handler.sphereCollider.enabled = true;
        cannonBallPool.Enqueue(obj);
    }
}
