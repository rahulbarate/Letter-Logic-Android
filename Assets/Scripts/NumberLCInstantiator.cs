using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class NumberLCInstantiator : MonoBehaviour
{
    [SerializeField] GameObject allNumbers;
    [SerializeField] GameObject letterCubeCopy;
    [SerializeField] GameObject slotSensors;
    [SerializeField] GameObject invisibleCharacter;
    [SerializeField] GameObject vCam;
    [SerializeField] float letterCubeScale = 97f;
    [SerializeField] NumberSubtype numberSubtype = NumberSubtype.E_Numbers;
    GameObject numberCopy;
    GameObject newLetterCube;
    LetterCubeData newLCData;
    LetterCubeHandler newLCHandler;
    SlotSensorsHandler slotSensorsHandler;

    List<int> availableNumbers;

    CinemachineFreeLook cineFreeCam;
    // Start is called before the first frame update
    void Start()
    {
        GameDataSave.IsLevelCompleted = false;
        cineFreeCam = vCam.GetComponent<CinemachineFreeLook>();
        GameDataSave.PlaygroundType = PlaygroundType.Numbers;
        GameDataSave.NumberSubtype = numberSubtype;
        slotSensorsHandler = slotSensors.GetComponent<SlotSensorsHandler>();
        GenerateAllNumbers();
        InstantiateLetterCube();
    }
    void GenerateAllNumbers()
    {
        availableNumbers = new List<int>();
        if (numberSubtype == NumberSubtype.E_Numbers)
        {
            for (int i = 1; i <= 10; i++)
            {
                availableNumbers.Add(i);
            }
            slotSensorsHandler.AssignENumbersToSlotSensors();
        }
    }

    private void InstantiateLetterCube()
    {
        if (availableNumbers.Count != 0)
        {
            //generate random index
            int randomIndex = UnityEngine.Random.Range(0, availableNumbers.Count);
            int numberToFetch = availableNumbers[randomIndex] - 1;

            //fetch number;
            numberCopy = allNumbers.transform.GetChild(numberToFetch).gameObject;

            //instantiating LetterCube and setting scale
            newLetterCube = Instantiate(letterCubeCopy, transform.position, Quaternion.identity);
            newLetterCube.transform.localScale = new Vector3(letterCubeScale, letterCubeScale, letterCubeScale);

            //checking if letter cube has data script, else adding it.
            if (newLetterCube.GetComponent<LetterCubeData>() == null)
            {
                newLetterCube.AddComponent<LetterCubeData>();
            }
            newLCData = newLetterCube.GetComponent<LetterCubeData>();

            //checking if letter cube has handler script, else adding it.
            if (newLetterCube.GetComponent<LetterCubeHandler>() == null)
            {
                newLetterCube.AddComponent<LetterCubeHandler>();
            }
            newLCHandler = newLetterCube.GetComponent<LetterCubeHandler>();

            newLCData.SetLetterOnCube(availableNumbers[randomIndex].ToString(), numberCopy);
            availableNumbers.RemoveAt(randomIndex);
            GameDataSave.LetterCubeData = newLCData;

            // setting camera to follow newly created Letter Cube.
            cineFreeCam.Follow = newLetterCube.transform;
            cineFreeCam.LookAt = newLetterCube.transform;
        }
        else
        {
            Debug.Log("Level Completed");
            GameDataSave.IsLevelCompleted = true;
            GameDataSave.LetterCubeData = null;
            cineFreeCam.Follow = invisibleCharacter.transform;
            cineFreeCam.LookAt = invisibleCharacter.transform;
            invisibleCharacter.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameDataSave.IsLevelCompleted)
        {
            LetterCubeState newLCState = newLCData.GetLetterCubeState();

            // checking if Letter Cube state is changed.
            if (newLCState == LetterCubeState.Matched)
            {
                Debug.Log("here");
                newLCHandler.ProcessCorrectLetterCube();
                InstantiateLetterCube();
            }
            else if (newLCState == LetterCubeState.Mismatched)
            {
                newLCHandler.ProcessIncorrectLetterCube(transform.localPosition);
            }
            else if (newLCState == LetterCubeState.Bombed)
            {
                newLCHandler.ProcessBombedLetterCube(transform.localPosition);
            }
        }
    }
}
