using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetSpawner : Spawner
{
    List<char> availableLetters;
    [SerializeField] GameObject alphabetLetterCubes;
    // Start is called before the first frame update
    void Start()
    {
        GenerateAllLetters();
        letterCubeMovement = GetComponent<LetterCubeMovement>();
        slotSensorsHandler.AssignCLettersToSlotSensors();
        SpawnLetterCubes();
        // InstantiateLetterCube();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void GenerateAllLetters()
    {
        availableLetters = new List<char>();

        for (char i = 'A'; i <= 'D'; i++)
            availableLetters.Add(i);
    }
    void SpawnLetterCubes()
    {
        if (availableLetters.Count != 0)
        {
            // generating random index and calculating letter cube to fetch from 26 letter cubes
            int randomLetterIndex = UnityEngine.Random.Range(0, availableLetters.Count);
            int letterCubeToFetch = 26 - (90 - Convert.ToInt32(availableLetters[randomLetterIndex])) - 1;

            //getting letter string
            letterChoosen = availableLetters[randomLetterIndex].ToString();

            activeLetterCube = alphabetLetterCubes.transform.GetChild(letterCubeToFetch).gameObject;

            activeLetterCube.transform.localScale = new UnityEngine.Vector3(0.95f, 0.95f, 0.95f);

            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot += OnPlacedInSlot;

            activeLetterCube.GetComponent<LetterCubeData>().LetterOnTop = letterChoosen;

            activeLetterCube.transform.position = transform.position;

            activeLetterCube.GetComponent<Rigidbody>().isKinematic = false;
            activeLetterCube.GetComponent<Rigidbody>().useGravity = true;

            availableLetters.RemoveAt(randomLetterIndex);

            // setting camera to follow newly created Letter Cube.
            cineFreeCam.Follow = activeLetterCube.transform;
            cineFreeCam.LookAt = activeLetterCube.transform;

            // set letter cube active
            activeLetterCube.SetActive(true);
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

            // making sure no forces are applied
            activeLetterCube.GetComponent<Rigidbody>().isKinematic = true;
            activeLetterCube.GetComponent<Rigidbody>().useGravity = false;

            // remove event listening.
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot -= OnPlacedInSlot;

            // setting isPlaced to true, so bombs won't affect it.
            activeLetterCube.GetComponent<LetterCubeData>().isPlaced = true;

            //Reset vars
            single3DLetterModel = null;
            activeLetterCube = null;
            letterChoosen = null;
            activeLetterCubeEventHandler = null;
            letterCubeMovement.ActiveLetterCube = null;

            SpawnLetterCubes();

            // InstantiateLetterCube();
        }
        else
        {
            // Process incorrect Letter Cube placement
            Debug.Log("Incorrect Letter Cube");
            // activeLetterCubeEventHandler.ProcessIncorrectLetterCube();
            letterCubeMovement.MoveToInitialPosition();
        }
    }
}
