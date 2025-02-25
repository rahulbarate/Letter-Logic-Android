using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCubeEventHandler : MonoBehaviour
{
    LetterCubeData letterCubeData;

    Rigidbody rgbody;
    [SerializeField] Transform sideLetters;
    [SerializeField] Transform sideLawnLayers;
    LetterCubeMovement letterCubeMovement;
    Transform slotSensor;

    //Event Register
    public event Action E_PlacedInSlot;
    public event Action E_IncorrectSlot;
    public event Action E_LetterCubeBombed;



    // Start is called before the first frame update
    void Start()
    {
        letterCubeData = GetComponent<LetterCubeData>();
        rgbody = GetComponent<Rigidbody>();
        letterCubeMovement = GetComponent<LetterCubeMovement>();
    }


    public void ProcessCorrectLetterCube()
    {
        // Debug.Log("Correct Letter Cube");
        Destroy(rgbody);
        Destroy(sideLetters.gameObject);
        Destroy(sideLawnLayers.gameObject);
        Destroy(letterCubeMovement);
        // Destroy(slotSensor.gameObject);
        // slotSensor.gameObject.SetActive(false);
        Destroy(this);
    }
    public void ProcessIncorrectLetterCube()
    {
        Debug.Log("Incorrect Letter Cube");
        letterCubeMovement.MoveToInitialPosition();
        // letterCubeData.SetLetterCubeState(LetterCubeState.Idle);
    }
    public void ProcessBombedLetterCube()
    {
        // Debug.Log("Letter Cube Bombed");
        letterCubeMovement.MoveToInitialPosition();
        // letterCubeData.SetLetterCubeState(LetterCubeState.Idle);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.tag);
        if (other.CompareTag("Slot Senser"))
        {
            Debug.Log(other.tag);
            // slotSensor = other.transform;
            E_PlacedInSlot?.Invoke();
            ProcessCorrectLetterCube();

            // if (slotSensor.GetComponent<SlotSensorData>().Letter == letterCubeData.GetLetterOnCube())
            // {
            //     // letterCubeData.SetLetterCubeState(LetterCubeState.Matched);

            //     E_PlacedInSlot?.Invoke();
            //     ProcessCorrectLetterCube();

            // }
            // else
            // {
            //     // letterCubeData.SetLetterCubeState(LetterCubeState.Mismatched);
            //     E_IncorrectSlot?.Invoke();
            //     ProcessIncorrectLetterCube();
            // }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bomb"))
        {
            // Debug.Log("tag: " + other.gameObject.tag);
            // letterCubeData.SetLetterCubeState(LetterCubeState.Bombed);
            E_LetterCubeBombed?.Invoke();
            ProcessBombedLetterCube();
        }
    }
}
