using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCubeHandler : MonoBehaviour
{
    LetterCubeData letterCubeData;

    Rigidbody rgbody;
    LetterCubeHandler letterCubeHandler;
    [SerializeField] Transform sideLetters;
    [SerializeField] Transform sideLawnLayers;
    LetterCubeMovement letterCubeMovement;
    Transform slotSensor;



    // Start is called before the first frame update
    void Start()
    {
        letterCubeData = GetComponent<LetterCubeData>();
        rgbody = GetComponent<Rigidbody>();
        letterCubeMovement = GetComponent<LetterCubeMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ProcessCorrectLetterCube()
    {
        Debug.Log("Correct Letter Cube");
        Destroy(rgbody);
        Destroy(letterCubeHandler);
        Destroy(sideLetters.gameObject);
        Destroy(sideLawnLayers.gameObject);
        Destroy(letterCubeMovement);
        Destroy(slotSensor.gameObject);
        Destroy(this);
    }
    public void ProcessIncorrectLetterCube(Vector3 pos)
    {
        Debug.Log("Incorrect Letter Cube");
        letterCubeMovement.MoveTo(pos);
        letterCubeData.SetLetterCubeState(LetterCubeState.Idle);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.tag);
        if (other.CompareTag("Slot Senser"))
        {
            slotSensor = other.transform;

            if (slotSensor.GetComponent<SlotSensorData>().Letter == letterCubeData.GetLetterOnCube())
            {
                GetComponent<LetterCubeData>().SetLetterCubeState(LetterCubeState.Matched);

            }
            else
            {
                GetComponent<LetterCubeData>().SetLetterCubeState(LetterCubeState.Mismatched);
            }
        }
    }
}
