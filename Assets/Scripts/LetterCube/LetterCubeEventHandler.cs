using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCubeEventHandler : MonoBehaviour
{
    LetterCubeData letterCubeData;

    Rigidbody rgbody;
    LetterCubeMovement letterCubeMovement;
    Transform slotSensor;

    //Event Register
    public event Action<string> E_PlacedInSlot;
    public event Action<GameObject> E_LetterCubeBombed;
    public event Action<GameObject> E_LetterCubeFell;



    // Start is called before the first frame update
    void Start()
    {
        letterCubeData = GetComponent<LetterCubeData>();
        rgbody = GetComponent<Rigidbody>();
        letterCubeMovement = GetComponent<LetterCubeMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.tag);
        if (other.CompareTag("Slot Senser"))
        {
            E_PlacedInSlot?.Invoke(other.GetComponent<SlotSensorHandler>().letter);
            transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bomb"))
        {
            E_LetterCubeBombed?.Invoke(this.gameObject);
            // ProcessBombedLetterCube();
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            E_LetterCubeFell?.Invoke(this.gameObject);
        }
    }
}
