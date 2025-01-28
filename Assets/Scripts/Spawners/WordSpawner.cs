using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Graphs;
using UnityEngine;

public class WordSpawner : MonoBehaviour
{
    [SerializeField] int wordLength = 3;
    [SerializeField] int numberOfWords = 3;
    [SerializeField] GameObject letterCubeCopy;
    [SerializeField] GameObject alphabetLetters;
    [SerializeField] GameObject firstSpawnPoint;
    [SerializeField] GameObject slotSensors;
    [SerializeField] GameObject vCam;
    [SerializeField] GameObject cinemachTargetObject;
    [SerializeField] float letterCubeScale = 97f;
    [SerializeField] float distanceBetweenSpawnPoints = 1.5f;
    [SerializeField] GameDataSave gameDataSave;
    GameObject instantiatedLetterCube;
    GameObject activeLetterCube;
    SlotSensorsHandler slotSensorsHandler;

    List<GameObject> instantiatedLCList;
    int[] placedLCList;
    List<Vector3> spawnPoints;
    List<Word> words;
    // List<char> userCreatedWord;
    CinemachineFreeLook cineFreeCam;
    CinemachineTargetGroup cinemachTargetGroup;

    int wordIndex;
    int activeLetterCubeIndex;
    // int lastActiveLetterCubeIndex;

    // Start is called before the first frame update
    void Start()
    {
        // Objects initializations
        cineFreeCam = vCam.GetComponent<CinemachineFreeLook>();
        // cinemachTargetGroup = cinemachTargetObject.GetComponent<CinemachineTargetGroup>();
        instantiatedLCList = new List<GameObject>(wordLength);
        spawnPoints = new List<Vector3>(wordLength);
        // wordChars = new List<char>(wordLength);
        slotSensorsHandler = slotSensors.GetComponent<SlotSensorsHandler>();
        // userCreatedWord = new List<char>(wordLength);
        activeLetterCubeIndex = 0;

        //subscribing to event
        gameDataSave.E_WordCompleted += WordCompleted;

        gameDataSave.WordLength = wordLength;
        gameDataSave.IsWordCompleted = false;

        // calculating spawn points
        CalculateSpawnPoints();

        // getting words from database
        words = DatabaseManger.GetWords(wordLength, numberOfWords);
        wordIndex = 0;

        // Instantiating first word
        InstantiateWord();
    }

    private void WordCompleted()
    {
        // check if the word is correct
        // Debug.Log("GetUserCreatedWord " + string.Concat(gameDataSave.GetUserCreatedWord()));
        // Debug.Log("words[wordIndex].Text " + words[wordIndex].Text);
        if (string.Concat(gameDataSave.GetUserCreatedWord()) == words[wordIndex].Text)
        {
            Debug.Log("Correct Word");
            wordIndex++;
            InstantiateWord();
        }
        else
        {
            Debug.Log("Incorrect Word");
            // Debug.Log("Press R to retry");
            // Debug.Log("Press N for next word");
        }
        // throw new NotImplementedException();
    }

    private void DestroyLetterCubes()
    {
        foreach (GameObject item in instantiatedLCList)
        {
            Destroy(item);
        }
    }

    void CalculateSpawnPoints()
    {
        spawnPoints.Clear();
        spawnPoints.Add(firstSpawnPoint.transform.position);
        for (int i = 1; i < wordLength; i++)
        {
            float xVal = spawnPoints[i - 1].x + distanceBetweenSpawnPoints;
            Vector3 nextSpawnPoint = new Vector3(xVal, firstSpawnPoint.transform.position.y, firstSpawnPoint.transform.position.z);
            spawnPoints.Add(nextSpawnPoint);
        }
    }

    private void InstantiateWord()
    {
        if (wordIndex > words.Count - 1)
        {
            Debug.Log("All words completed");
            return;
        }
        DestroyLetterCubes();
        Word word = words[wordIndex];
        List<char> wordChars = new List<char>(word.Text.ToCharArray());
        instantiatedLCList.Clear();
        placedLCList = new int[wordLength];
        activeLetterCubeIndex = 0;
        gameDataSave.IsWordCompleted = false;
        gameDataSave.InitializeUserCreatedWord();

        Debug.Log("Hint:" + word.Hint);

        // spawning Letter Cubes for each letter in word
        for (int i = 0; i < word.TextLength; i++)
        {
            int randomCharIndex = UnityEngine.Random.Range(0, wordChars.Count);

            // to avoid first letter at the first position(specially for 3 letter words)
            while (i == 0 && randomCharIndex == 0)
            {
                randomCharIndex = UnityEngine.Random.Range(0, wordChars.Count);
            }


            int letterToFetch = 26 - (90 - Convert.ToInt32(Char.ToUpper(wordChars[randomCharIndex]))) - 1;
            GameObject alphabetLetterCopy = alphabetLetters.transform.GetChild(letterToFetch).gameObject;

            instantiatedLetterCube = LetterCubeInstantiator.InstantiateLetterCube(letterCubeCopy, spawnPoints[i], letterCubeScale, wordChars[randomCharIndex].ToString(), alphabetLetterCopy, false);
            instantiatedLetterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot += OnPlacedInSlot;
            instantiatedLCList.Insert(i, instantiatedLetterCube);
            wordChars.RemoveAt(randomCharIndex);
        }
        // activeLetterCube = instantiatedLCList[activeLetterCubeIndex];
        // slotSensorsHandler.SetAllActive();
        ActivateLetterCube();
    }

    private void OnPlacedInSlot()
    {
        if (!gameDataSave.IsWordCompleted)
        {
            // Debug.Log("activeLetterCubeIndex: " + activeLetterCubeIndex);
            // Debug.Log("instantiatedLCList.Count: " + instantiatedLCList.Count);
            placedLCList[activeLetterCubeIndex] = 1;

            // placedLCList.Add(instantiatedLCList[activeLetterCubeIndex]);
            // instantiatedLCList.RemoveAt(activeLetterCubeIndex);
            activeLetterCubeIndex++;
            if (activeLetterCubeIndex >= instantiatedLCList.Count)
            {
                activeLetterCubeIndex = GetNextActiveElement();
            }
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot -= OnPlacedInSlot;
            ActivateLetterCube();
        }

    }

    private int GetNextActiveElement()
    {
        for (int i = 0; i < wordLength; i++)
        {
            if (placedLCList[i] == 0)
            {
                return i;
            }
        }
        return 0;
    }

    private void DeactivateLetterCube()
    {

        if (activeLetterCube != null && activeLetterCube.TryGetComponent<LetterCubeMovement>(out LetterCubeMovement letterCubeMovement))
        {
            letterCubeMovement.enabled = false;
        }
    }

    private void ActivateLetterCube()
    {
        DeactivateLetterCube();
        activeLetterCube = instantiatedLCList[activeLetterCubeIndex];
        if (activeLetterCube.TryGetComponent<LetterCubeMovement>(out LetterCubeMovement letterCubeMovement))
        {

            activeLetterCube.GetComponent<LetterCubeMovement>().enabled = true;
            cineFreeCam.Follow = activeLetterCube.transform;
            cineFreeCam.LookAt = activeLetterCube.transform;
        }
        else
        {

            Debug.Log($"Letter Cube at index {activeLetterCubeIndex + 1} is not active");
            Debug.Log($"Press Q to switch to the next Letter Cube");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown((KeyCode)(int)KeyCode.Alpha1 + i) || Input.GetKeyDown((KeyCode)(int)KeyCode.Keypad1 + i))
            {
                if (i < instantiatedLCList.Count)
                {
                    activeLetterCubeIndex = i;
                    ActivateLetterCube();
                }
                else
                {
                    Debug.Log($"No Letter Cube at index {i + 1}");
                }
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            activeLetterCubeIndex++;
            if (activeLetterCubeIndex >= instantiatedLCList.Count)
            {
                activeLetterCubeIndex = GetNextActiveElement();
            }
            ActivateLetterCube();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            InstantiateWord();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            wordIndex++;
            InstantiateWord();
        }
    }
}
