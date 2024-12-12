using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class NumberLCSpawner : MonoBehaviour
{
    [SerializeField] GameObject allNumbers;
    [SerializeField] GameObject letterCubeCopy;
    [SerializeField] GameObject slotSensors;
    [SerializeField] GameObject invisibleCharacter;
    [SerializeField] GameObject vCam;
    [SerializeField] GameDataSave gameDataSave;
    [SerializeField] float letterCubeScale = 97f;
    [SerializeField] NumberSubtype numberSubtype = NumberSubtype.E_Numbers;
    GameObject numberCopy;
    GameObject activeLetterCube;
    LetterCubeData activeLCData;
    LetterCubeEventHandler activeLCEventHandler;
    SlotSensorsHandler slotSensorsHandler;

    List<int> availableNumbers;

    CinemachineFreeLook cineFreeCam;
    // Start is called before the first frame update
    void Start()
    {
        gameDataSave.IsLevelCompleted = false;
        cineFreeCam = vCam.GetComponent<CinemachineFreeLook>();
        gameDataSave.PlaygroundType = PlaygroundType.Numbers;
        gameDataSave.NumberSubtype = numberSubtype;
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
            // slotSensorsHandler.AssignENumbersToSlotSensors();
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
            string number = availableNumbers[randomIndex].ToString();

            //instantiating LetterCube and setting scale
            activeLetterCube = LetterCubeInstantiator.InstantiateLetterCube(letterCubeCopy, transform.position, letterCubeScale, number, numberCopy);

            //Subscribe to event
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_CorrectSlot += OnCorrectLCPlaced;

            // activeLCData.SetLetterOnCube(availableNumbers[randomIndex].ToString(), numberCopy);
            availableNumbers.RemoveAt(randomIndex);
            gameDataSave.LetterCube = activeLetterCube;

            // setting camera to follow newly created Letter Cube.
            cineFreeCam.Follow = activeLetterCube.transform;
            cineFreeCam.LookAt = activeLetterCube.transform;
            slotSensorsHandler.SetActive(numberToFetch);
        }
        else
        {
            Debug.Log("Level Completed");
            gameDataSave.IsLevelCompleted = true;
            gameDataSave.LetterCube = null;
            cineFreeCam.Follow = invisibleCharacter.transform;
            cineFreeCam.LookAt = invisibleCharacter.transform;
            invisibleCharacter.SetActive(true);
        }
    }

    private void OnCorrectLCPlaced()
    {
        activeLetterCube.GetComponent<LetterCubeEventHandler>().E_CorrectSlot -= OnCorrectLCPlaced;
        InstantiateLetterCube();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (!gameDataSave.IsLevelCompleted)
    //     {
    //         LetterCubeState newLCState = activeLCData.GetLetterCubeState();

    //         // checking if Letter Cube state is changed.
    //         if (newLCState == LetterCubeState.Matched)
    //         {
    //             Debug.Log("here");
    //             activeLCEventHandler.ProcessCorrectLetterCube();
    //             InstantiateLetterCube();
    //         }
    //         else if (newLCState == LetterCubeState.Mismatched)
    //         {
    //             // activeLCEventHandler.ProcessIncorrectLetterCube(transform.localPosition);
    //         }
    //         else if (newLCState == LetterCubeState.Bombed)
    //         {
    //             // activeLCEventHandler.ProcessBombedLetterCube(transform.localPosition);
    //         }
    //     }
    // }
}
