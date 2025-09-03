using System;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using UnityEngine;

public class WordSpawner : MonoBehaviour
{
    [SerializeField] int textLength = 3;
    [SerializeField] int numberOfWords = 3;
    [SerializeField] GameObject alphabetLetterCubes;
    [SerializeField] GameObject firstSpawnPoint;
    [SerializeField] CinemachineFreeLook cineFreeCam;
    [SerializeField] float distanceBetweenSpawnPoints = 1.5f;
    [SerializeField] GameDataSave gameDataSave;
    [SerializeField] PlaygroundType playgroundType = PlaygroundType.Words;
    [SerializeField] SlotSensorsHandler slotSensorsHandler;

    List<UnityEngine.Vector3> spawnPoints;
    List<Word> words;
    DatabaseManager databaseManager;
    int activeLetterCubeIndex;
    // int currentWordIndex;
    Word wordChosen;
    List<char> wordChosenInChars;
    List<GameObject> letterCubesForChosenWord;
    List<GameObject> instantiatedLetterCubes;

    int correctlyPlacedLCCount = 0;

    GameObject activeLetterCube;
    // int activeLetterIndex;

    LetterCubeMovement letterCubeMovement;

    void Start()
    {
        databaseManager = new DatabaseManager("wordsDatabase.db");
        spawnPoints = new List<UnityEngine.Vector3>(textLength);
        words = databaseManager.GetWordsFromDatabase(textLength, numberOfWords);
        letterCubeMovement = GetComponent<LetterCubeMovement>();
        letterCubesForChosenWord = new();
        instantiatedLetterCubes = new List<GameObject>(26);

        activeLetterCubeIndex = 0;
        // currentWordIndex = -1;

        CalculateSpawnPoints();
        SpawnLetterCubes();
    }
    void CalculateSpawnPoints()
    {
        spawnPoints.Clear();
        spawnPoints.Add(firstSpawnPoint.transform.position);
        for (int i = 1; i < textLength; i++)
        {
            float xVal = spawnPoints[i - 1].x + distanceBetweenSpawnPoints;
            UnityEngine.Vector3 nextSpawnPoint = new UnityEngine.Vector3(xVal, firstSpawnPoint.transform.position.y, firstSpawnPoint.transform.position.z);
            spawnPoints.Add(nextSpawnPoint);
        }
    }

    void SpawnLetterCubes()
    {
        // Hide previous letter cubes if any
        ToggleLetterCubesVisibility(false);

        // reset common vars
        correctlyPlacedLCCount = 0;
        activeLetterCubeIndex = 0;
        wordChosen = null;
        wordChosenInChars = null;
        letterCubesForChosenWord = null;

        // choose the next word in line
        if (words.Count == 0)
        {
            CustomLogger.Log("No more words to spawn.");
            return;
        }
        wordChosen = words[0];
        words.RemoveAt(0);

        // split it into characters
        wordChosenInChars = new List<char>(wordChosen.Text.ToCharArray());

        //Display Hint
        CustomLogger.Log("Hint:" + wordChosen.Hint);
        //Assign Letters to the Slot Sensors;
        slotSensorsHandler.AssignWord(wordChosenInChars);

        // initialize the letter cubes holder
        letterCubesForChosenWord = new();

        for (int i = 0; i < wordChosen.TextLength; i++)
        {
            int randomCharIndex = UnityEngine.Random.Range(0, wordChosenInChars.Count);

            // to avoid first letter at the first position(specially for 3 letter words)
            while (i == 0 && randomCharIndex == 0)
            {
                randomCharIndex = UnityEngine.Random.Range(0, wordChosenInChars.Count);
            }
            int letterCubeToFetch = 26 - (90 - Convert.ToInt32(Char.ToUpper(wordChosenInChars[randomCharIndex]))) - 1;
            GameObject letterCube = alphabetLetterCubes.transform.GetChild(letterCubeToFetch).gameObject;
            if (letterCubesForChosenWord.Contains(letterCube))
            {
                // CustomLogger.Log(letterCubeToFetch);
                // CustomLogger.Log(instantiatedLetterCubes[letterCubeToFetch]);
                if (letterCubeToFetch < instantiatedLetterCubes.Count && instantiatedLetterCubes[letterCubeToFetch] != null)
                    letterCube = instantiatedLetterCubes[letterCubeToFetch];
                else
                {
                    letterCube = Instantiate(letterCube);
                    // Ensure the list has enough elements
                    while (instantiatedLetterCubes.Count <= letterCubeToFetch)
                        instantiatedLetterCubes.Add(null);
                    instantiatedLetterCubes[letterCubeToFetch] = letterCube;
                }

                letterCube.transform.localScale = new UnityEngine.Vector3(97f, 97f, 97f);
            }
            else
                letterCube.transform.localScale = new UnityEngine.Vector3(0.95f, 0.95f, 0.95f);
            letterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot += OnPlacedInSlot;
            letterCube.GetComponent<LetterCubeData>().SetLetterOnCube(wordChosenInChars[randomCharIndex].ToString());
            letterCube.transform.position = spawnPoints[i];
            letterCube.GetComponent<Rigidbody>().isKinematic = true;
            letterCube.GetComponent<Rigidbody>().useGravity = false;
            letterCubesForChosenWord.Add(letterCube);
            wordChosenInChars.RemoveAt(randomCharIndex);
        }

        // // Resposition the Letter Cubes to spawn points
        // for (int i = 0; i < wordChosen.TextLength; i++)
        // {
        //     letterCubesForChosenWord[i].transform.position = spawnPoints[i];
        // }

        // Un-hide the new Letter Cubes
        ToggleLetterCubesVisibility(true);

        //Set the Letter Cube Active
        SetLetterCubeActive(0);
    }

    void RespawnLetterCubes()
    {
        // Hide all the Letter Cubes
        ToggleLetterCubesVisibility(false);

        // reset common vars
        correctlyPlacedLCCount = 0;
        activeLetterCubeIndex = 0;

        for (int i = 0; i < wordChosen.TextLength; i++)
        {
            // set isPlaced to false
            letterCubesForChosenWord[i].GetComponent<LetterCubeData>().isPlaced = false;
            // reposition them to the spawn points
            letterCubesForChosenWord[i].transform.position = spawnPoints[i];
            // re register the event
            letterCubesForChosenWord[i].GetComponent<LetterCubeEventHandler>().E_PlacedInSlot += OnPlacedInSlot;
            // set rigidbody is kinematic to true, so cube isn't moveable if it is not active.
            letterCubesForChosenWord[i].GetComponent<Rigidbody>().isKinematic = true;
            // disable the gravity
            letterCubesForChosenWord[i].GetComponent<Rigidbody>().useGravity = false;



        }

        // Un-hide the Letter Cubes
        ToggleLetterCubesVisibility(true);

        //Set the Letter Cube Active
        SetLetterCubeActive(0);
    }

    void ToggleLetterCubesVisibility(bool visibility)
    {
        if (letterCubesForChosenWord.Count > 0)
        {
            foreach (GameObject letterCube in letterCubesForChosenWord)
            {
                letterCube.SetActive(visibility);
            }
        }
    }

    void OnPlacedInSlot(string letterOnSlotSensor)
    {
        // CustomLogger.Log($"Cube placed; letterOnSlotSensor: {letterOnSlotSensor}, letterOnCube: {activeLetterCube.GetComponent<LetterCubeData>().GetLetterOnCube()}");


        // setting isPlaced to true so they won't be affected by bombing.
        activeLetterCube.GetComponent<LetterCubeData>().isPlaced = true;
        activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot -= OnPlacedInSlot;
        activeLetterCube.GetComponent<Rigidbody>().isKinematic = true;
        activeLetterCube.GetComponent<Rigidbody>().useGravity = false;

        // for (int i = 0; i <= textLength - 1; i++)
        // {
        //     Debug.Log(wordChosenInChars[activeLetterCubeIndex].ToString());
        // }
        if (letterOnSlotSensor == activeLetterCube.GetComponent<LetterCubeData>().GetLetterOnCube())
        {
            // Debug.Log($"Correctly placed; letterOnSlotSensor:{letterOnSlotSensor} == {activeLetterCube.GetComponent<LetterCubeData>().GetLetterOnCube()}");
            correctlyPlacedLCCount++;
        }
        else
        {
            // Debug.Log($"Correctly placed; letterOnSlotSensor:{letterOnSlotSensor} != {activeLetterCube.GetComponent<LetterCubeData>().GetLetterOnCube()}");
            correctlyPlacedLCCount--;
        }

        // check if there are any Letter Cubes yet to be placed.
        if (activeLetterCubeIndex == textLength - 1)
        {
            // Debug.Log($"Last LC Placed; activeLetterCubeIndex:{activeLetterCubeIndex}, correctlyPlacedLCCount:{correctlyPlacedLCCount}, textLength:{textLength}");
            // Check if all letter cubes placed correct or not.
            if (correctlyPlacedLCCount == textLength)
            {
                CustomLogger.Log("Correct word");

                //spawn next word
                if (words.Count > 0)
                    SpawnLetterCubes();
                else
                    CustomLogger.Log("Level Completed");
            }
            else
            {
                CustomLogger.LogWarning("Incorrect word");
                RespawnLetterCubes();
            }
        }
        else
        {
            // change the active letter cube as the previous one is placed.
            activeLetterCubeIndex += 1;
            SetLetterCubeActive(activeLetterCubeIndex);

        }
    }

    void SetLetterCubeActive(int letterIndex)
    {
        activeLetterCube = letterCubesForChosenWord[letterIndex];
        letterCubeMovement.ActiveLetterCube = activeLetterCube;
        activeLetterCube.GetComponent<Rigidbody>().isKinematic = false;
        activeLetterCube.GetComponent<Rigidbody>().useGravity = true;
        // activeLetterCubeIndex = letterIndex;
        cineFreeCam.Follow = letterCubesForChosenWord[letterIndex].transform;
        cineFreeCam.LookAt = letterCubesForChosenWord[letterIndex].transform;
    }

}