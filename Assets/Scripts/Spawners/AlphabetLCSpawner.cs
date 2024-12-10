using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AlphabetLCSpawner : MonoBehaviour
{
    [SerializeField] GameObject allLetters;
    [SerializeField] GameObject letterCubeCopy;
    [SerializeField] GameObject invisibleCharacter;
    [SerializeField] GameObject vCam;
    [SerializeField] GameObject slotSensors;
    [SerializeField] float letterCubeScale = 97f;
    [SerializeField] AlphabetSubtype alphabetSubtype = AlphabetSubtype.C_Letters;
    [SerializeField] GameDataSave gameDataSave;
    GameObject letterCopy;
    GameObject activeLetterCube;
    // LetterCubeData activeLCData;
    LetterCubeEventHandler activeLCEventHandler;

    SlotSensorsHandler slotSensorsHandler;

    List<char> availableLetters;

    CinemachineFreeLook cineFreeCam;
    // bool isLevelCompleted;

    // public bool IsLevelCompleted
    // {
    //     get { return isLevelCompleted; }
    //     set { isLevelCompleted = value; }
    // }


    // Start is called before the first frame update
    void Start()
    {
        // gameDataSave.IsLevelCompleted = false;
        cineFreeCam = vCam.GetComponent<CinemachineFreeLook>();
        // gameDataSave.PlaygroundType = PlaygroundType.Alphabet;
        // gameDataSave.AlphabetSubtype = alphabetSubtype;
        slotSensorsHandler = slotSensors.GetComponent<SlotSensorsHandler>();
        GenerateAllLetters();
        InstantiateLetterCube();
    }
    private void GenerateAllLetters()
    {
        availableLetters = new List<char>();
        // if (alphabetSubtype == AlphabetSubtype.C_Letters)
        // {
        for (char i = 'A'; i <= 'D'; i++)
        {
            availableLetters.Add(i);
        }
        slotSensorsHandler.AssignCLettersToSlotSensors();

        // }

    }

    private void InstantiateLetterCube()
    {
        if (availableLetters.Count != 0)
        {
            // generating random index and calculating letter to fetch from 26 lettes
            int randomLetterIndex = UnityEngine.Random.Range(0, availableLetters.Count);
            int letterToFetch = 26 - (90 - Convert.ToInt32(availableLetters[randomLetterIndex])) - 1;
            //getting letter string
            string letter = availableLetters[randomLetterIndex].ToString();

            //fetching letter object to be displayed on the Cube.
            letterCopy = allLetters.transform.GetChild(letterToFetch).gameObject;
            //Instantiating Letter Cube
            activeLetterCube = LetterCubeInstantiator.InstantiateLetterCube(letterCubeCopy, transform.position, letterCubeScale, letter, letterCopy);

            //Subscribing to event
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_CorrectSlot += OnCorrectLCPlaced;

            availableLetters.RemoveAt(randomLetterIndex);
            gameDataSave.LetterCube = activeLetterCube;

            // setting camera to follow newly created Letter Cube.
            cineFreeCam.Follow = activeLetterCube.transform;
            cineFreeCam.LookAt = activeLetterCube.transform;

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

}
