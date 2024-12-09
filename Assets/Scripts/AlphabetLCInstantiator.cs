using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AlphabetLCInstantiator : MonoBehaviour
{
    [SerializeField] GameObject allLetters;
    [SerializeField] GameObject letterCubeCopy;
    [SerializeField] GameObject invisibleCharacter;
    [SerializeField] GameObject vCam;
    [SerializeField] GameObject slotSensors;
    [SerializeField] float letterCubeScale = 97f;
    [SerializeField] AlphabetSubtype alphabetSubtype = AlphabetSubtype.C_Letters;
    GameObject letterCopy;
    GameObject activeLetterCube;
    LetterCubeData activeLCData;
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
        GameDataSave.IsLevelCompleted = false;
        cineFreeCam = vCam.GetComponent<CinemachineFreeLook>();
        GameDataSave.PlaygroundType = PlaygroundType.Alphabet;
        GameDataSave.AlphabetSubtype = alphabetSubtype;
        slotSensorsHandler = slotSensors.GetComponent<SlotSensorsHandler>();
        GenerateAllLetters();
        InstantiateLetterCube();
    }
    private void GenerateAllLetters()
    {
        availableLetters = new List<char>();
        if (alphabetSubtype == AlphabetSubtype.C_Letters)
        {
            for (char i = 'A'; i <= 'D'; i++)
            {
                availableLetters.Add(i);
            }
            slotSensorsHandler.AssignCLettersToSlotSensors();

        }

    }

    private void InstantiateLetterCube()
    {
        if (availableLetters.Count != 0)
        {
            // generating random index and calculating letter to fetch from 26 lettes
            int randomLetterIndex = UnityEngine.Random.Range(0, availableLetters.Count);
            int letterToFetch = 26 - (90 - Convert.ToInt32(availableLetters[randomLetterIndex])) - 1;

            //fetching letter object to be displayed on the Cube.
            letterCopy = allLetters.transform.GetChild(letterToFetch).gameObject;


            // Instantiating letter and setting scale.
            activeLetterCube = Instantiate(letterCubeCopy, transform.position, Quaternion.identity);
            activeLetterCube.transform.localScale = new Vector3(letterCubeScale, letterCubeScale, letterCubeScale);

            //checking if letter cube has data script, else adding it.
            if (activeLetterCube.GetComponent<LetterCubeData>() == null)
            {
                activeLetterCube.AddComponent<LetterCubeData>();
            }
            activeLCData = activeLetterCube.GetComponent<LetterCubeData>();

            //checking if letter cube has handler script, else adding it.
            if (activeLetterCube.GetComponent<LetterCubeEventHandler>() == null)
            {
                activeLetterCube.AddComponent<LetterCubeEventHandler>();
            }
            activeLCEventHandler = activeLetterCube.GetComponent<LetterCubeEventHandler>();

            //Subscribing to event
            activeLCEventHandler.E_CorrectSlot += OnCorrectLCPlaced;
            // activeLCEventHandler.E_IncorrectSlot += OnInCorrectLCPlaced;
            // activeLCEventHandler.E_LetterCubeBombed += OnLCBombed;



            // setting letter to be displayed on top and storing it in data script.
            activeLCData.SetLetterOnCube(availableLetters[randomLetterIndex].ToString(), letterCopy);
            availableLetters.RemoveAt(randomLetterIndex);
            GameDataSave.LetterCubeData = activeLCData;

            // setting camera to follow newly created Letter Cube.
            cineFreeCam.Follow = activeLetterCube.transform;
            cineFreeCam.LookAt = activeLetterCube.transform;




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

    // private void OnLCBombed()
    // {
    //     throw new NotImplementedException();
    // }

    // private void OnInCorrectLCPlaced()
    // {
    //     throw new NotImplementedException();
    // }

    private void OnCorrectLCPlaced()
    {
        activeLCEventHandler.E_CorrectSlot -= OnCorrectLCPlaced;
        InstantiateLetterCube();
    }



    // Update is called once per frame
    // void Update()
    // {

    //     if (!GameDataSave.IsLevelCompleted)
    //     {
    //         LetterCubeState newLCState = activeLCData.GetLetterCubeState();

    //         // checking if Letter Cube state is changed.
    //         if (newLCState == LetterCubeState.Matched)
    //         {
    //             activeLCEventHandler.ProcessCorrectLetterCube();
    //             InstantiateLetterCube();
    //         }
    //         else if (newLCState == LetterCubeState.Mismatched)
    //         {
    //             activeLCEventHandler.ProcessIncorrectLetterCube(transform.localPosition);
    //         }
    //         else if (newLCState == LetterCubeState.Bombed)
    //         {
    //             activeLCEventHandler.ProcessBombedLetterCube(transform.localPosition);
    //         }
    //     }
    // }
}
