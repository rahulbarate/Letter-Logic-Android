using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSpawner : Spawner
{
    List<int> availableNumbers;
    // Start is called before the first frame update
    void Start()
    {
        GenerateAllLetters();
        letterCubeMovement = GetComponent<LetterCubeMovement>();
        slotSensorsHandler.AssignENumbersToSlotSensors();
        InstantiateLetterCube();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void GenerateAllLetters()
    {
        availableNumbers = new List<int>();

        for (int i = 1; i <= 10; i++)
            availableNumbers.Add(i);
    }
    private void InstantiateLetterCube()
    {
        if (availableNumbers.Count != 0)
        {
            // generating random index and calculating number to fetch from 10 numbers
            int randomNumberIndex = UnityEngine.Random.Range(0, availableNumbers.Count);
            int numberToFetch = availableNumbers[randomNumberIndex] - 1;

            //getting and coverting number to string
            letterChoosen = availableNumbers[randomNumberIndex].ToString();

            //fetching letter object to be displayed on the Cube.
            single3DLetterModel = letters3DModels.transform.GetChild(numberToFetch).gameObject;

            //Instantiating Letter Cube
            activeLetterCube = LetterCubeInstantiator.InstantiateLetterCube(letterCubeModel, transform.position, letterCubeScale, letterChoosen, single3DLetterModel);


            //Subscribing to event
            activeLetterCubeEventHandler = activeLetterCube.GetComponent<LetterCubeEventHandler>();
            activeLetterCubeEventHandler.E_PlacedInSlot += OnPlacedInSlot;

            availableNumbers.RemoveAt(randomNumberIndex);
            // gameDataSave.LetterCube = activeLetterCube;

            // setting camera to follow newly created Letter Cube.
            cineFreeCam.Follow = activeLetterCube.transform;
            cineFreeCam.LookAt = activeLetterCube.transform;

            letterCubeMovement.ActiveLetterCube = activeLetterCube;

        }
        else
        {
            Debug.Log("Level Completed");
            gameDataSave.IsLevelCompleted = true;
            gameDataSave.LetterCube = null;
        }

    }
    public override void OnPlacedInSlot(string letterOnSlotSensor)
    {
        if (letterOnSlotSensor == letterChoosen)
        {
            //Process correct Letter Cube placement

            // remove event listening.
            activeLetterCubeEventHandler.E_PlacedInSlot -= OnPlacedInSlot;

            // setting isPlaced to true, so bombs won't affect it.
            activeLetterCube.GetComponent<LetterCubeData>().isPlaced = true;

            //Start Correct Cube Sequence.
            // activeLetterCubeEventHandler.ProcessPlacedLetterCube();

            //Reset vars
            single3DLetterModel = null;
            activeLetterCube = null;
            letterChoosen = null;
            activeLetterCubeEventHandler = null;
            letterCubeMovement.ActiveLetterCube = null;

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
