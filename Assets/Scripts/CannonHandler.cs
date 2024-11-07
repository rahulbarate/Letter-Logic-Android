using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHandler : MonoBehaviour
{

    [SerializeField] GameObject cannonBallCopy;
    [SerializeField] GameObject cannonBallSpawnPoint;
    [SerializeField] float forceAmount = 500f;
    GameObject instantiatedCannonBall;
    Rigidbody cannonBallRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
      

    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     FireCannon();
        // }
    }
    public void FireCannon()
    {
        instantiatedCannonBall = Instantiate(cannonBallCopy, cannonBallSpawnPoint.transform.position, Quaternion.identity);
        instantiatedCannonBall.SetActive(true);
        instantiatedCannonBall.transform.localScale = new Vector3(27f, 27f, 27f);
        instantiatedCannonBall.AddComponent<Rigidbody>();
        cannonBallRigidbody = instantiatedCannonBall.GetComponent<Rigidbody>();
        Vector3 localForce = new Vector3(0f, 0f, forceAmount);
        Vector3 worldForce = transform.TransformDirection(localForce);
        cannonBallRigidbody.AddForce(worldForce, ForceMode.Impulse);
    }
}
