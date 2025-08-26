using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetSpawner : Spawner
{
    List<char> availableLetters;
    // Start is called before the first frame update
    void Start()
    {
        GenerateAllLetters();
        letterCubeMovement = GetComponent<LetterCubeMovement>();
        slotSensorsHandler.AssignCLettersToSlotSensors();
        InstantiateLetterCube();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void GenerateAllLetters()
    {
        availableLetters = new List<char>();

        for (char i = 'A'; i <= 'Z'; i++)
            availableLetters.Add(i);
    }
    private void InstantiateLetterCube()
    {
        if (availableLetters.Count != 0)
        {
            // generating random index and calculating letter to fetch from 26 lettes
            int randomLetterIndex = UnityEngine.Random.Range(0, availableLetters.Count);
            int letterToFetch = 26 - (90 - Convert.ToInt32(availableLetters[randomLetterIndex])) - 1;
            //getting letter string
            letterChoosen = availableLetters[randomLetterIndex].ToString();

            //fetching letter object to be displayed on the Cube.
            single3DLetterModel = letters3DModels.transform.GetChild(letterToFetch).gameObject;

            //Instantiating Letter Cube
            activeLetterCube = LetterCubeInstantiator.InstantiateLetterCube(letterCubeModel, transform.position, letterCubeScale, letterChoosen, single3DLetterModel);


            //Subscribing to event
            activeLetterCubeEventHandler = activeLetterCube.GetComponent<LetterCubeEventHandler>();
            activeLetterCubeEventHandler.E_PlacedInSlot += OnPlacedInSlot;

            availableLetters.RemoveAt(randomLetterIndex);
            // gameDataSave.LetterCube = activeLetterCube;

            // setting camera to follow newly created Letter Cube.
            cineFreeCam.Follow = activeLetterCube.transform;
            cineFreeCam.LookAt = activeLetterCube.transform;

            letterCubeMovement.activeLetterCube = activeLetterCube;


            // slotSensorsHandler.SetActive(letterToFetch);
            // slotSensorsHandler.SetActive(randomLetterIndex);

        }
        else
        {
            Debug.Log("Level Completed");
            gameDataSave.IsLevelCompleted = true;
            gameDataSave.LetterCube = null;
            // cineFreeCam.Follow = invisibleCharacter.transform;
            // cineFreeCam.LookAt = invisibleCharacter.transform;
            // invisibleCharacter.SetActive(true);

        }

    }
    public override void OnPlacedInSlot(string letterOnSlotSensor)
    {
        if (letterOnSlotSensor == letterChoosen)
        {
            //Process correct Letter Cube placement

            // remove event listening.
            activeLetterCubeEventHandler.E_PlacedInSlot -= OnPlacedInSlot;

            //Start Correct Cube Sequence.
            activeLetterCubeEventHandler.ProcessCorrectLetterCube();

            //Reset vars
            single3DLetterModel = null;
            activeLetterCube = null;
            letterChoosen = null;
            activeLetterCubeEventHandler = null;
            letterCubeMovement.activeLetterCube = null;

            InstantiateLetterCube();
        }
        else
        {
            // Process incorrect Letter Cube placement
            activeLetterCubeEventHandler.ProcessIncorrectLetterCube();
            letterCubeMovement.MoveToInitialPosition();
        }
    }
}
