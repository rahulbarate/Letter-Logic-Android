using System;
using System.Collections.Generic;
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

    List<Vector3> spawnPoints;
    List<Word> words;
    DatabaseManager databaseManager;
    int activeLetterCubeIndex;
    int currentWordIndex;
    Word wordChoosen;
    List<char> wordChoosenInChars;
    List<GameObject> letterCubesForChoosenWord;

    int correctlyPlacedLCCount = 0;

    GameObject activeLetterCube;
    int activeLetterIndex;

    LetterCubeMovement letterCubeMovement;

    void Start()
    {
        databaseManager = new DatabaseManager("wordsDatabase.db");
        spawnPoints = new List<Vector3>(textLength);
        words = databaseManager.GetWordsFromDatabase(textLength, numberOfWords);
        letterCubeMovement = GetComponent<LetterCubeMovement>();

        activeLetterCubeIndex = 0;
        currentWordIndex = -1;

        CalculateSpawnPoints();
        SpawnLetterCubes(false);
    }
    void CalculateSpawnPoints()
    {
        spawnPoints.Clear();
        spawnPoints.Add(firstSpawnPoint.transform.position);
        for (int i = 1; i < textLength; i++)
        {
            float xVal = spawnPoints[i - 1].x + distanceBetweenSpawnPoints;
            Vector3 nextSpawnPoint = new Vector3(xVal, firstSpawnPoint.transform.position.y, firstSpawnPoint.transform.position.z);
            spawnPoints.Add(nextSpawnPoint);
        }
    }

    void SpawnLetterCubes(bool respawnLetterCubes)
    {
        // Hide all the Letter Cubes
        ToggleLetterCubesVisibility(false);

        // Fetch the Letters for new Letter Cubes and change the Letter on tops
        if (respawnLetterCubes == false)
        {
            // choose the next word in line
            ++currentWordIndex;
            wordChoosen = words[currentWordIndex];
            words.RemoveAt(currentWordIndex);
            // split it into characters
            wordChoosenInChars = new List<char>(wordChoosen.Text.ToCharArray());
            //Display Hint
            Debug.Log("Hint:" + wordChoosen.Hint);
            //Assign Letters to the Slot Sensors;
            slotSensorsHandler.AssignWord(wordChoosenInChars);

            for (int i = 0; i < wordChoosen.TextLength; i++)
            {
                int randomCharIndex = UnityEngine.Random.Range(0, wordChoosenInChars.Count);

                // to avoid first letter at the first position(specially for 3 letter words)
                while (i == 0 && randomCharIndex == 0)
                {
                    randomCharIndex = UnityEngine.Random.Range(0, wordChoosenInChars.Count);
                }
                int letterCubeToFetch = 26 - (90 - Convert.ToInt32(Char.ToUpper(wordChoosenInChars[randomCharIndex]))) - 1;
                GameObject letterCube = alphabetLetterCubes.transform.GetChild(letterCubeToFetch).gameObject;
                letterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot += OnPlacedInSlot;
                letterCubesForChoosenWord.Add(letterCube);
                wordChoosenInChars.RemoveAt(randomCharIndex);
            }
        }

        // Resposition the Letter Cubes to spawn points
        for (int i = 0; i < wordChoosen.TextLength; i++)
        {
            letterCubesForChoosenWord[i].transform.localPosition = spawnPoints[i];
        }

        // Un-hide the Letter Cubes
        ToggleLetterCubesVisibility(true);

        //Set the 1st Letter Cube Active
        SetLetterCubeActive(0);

    }

    void ToggleLetterCubesVisibility(bool visibility)
    {
        foreach (GameObject letterCube in letterCubesForChoosenWord)
        {
            letterCube.SetActive(visibility);
        }
    }

    void OnPlacedInSlot(string letterOnSlotSensor)
    {
        if (letterOnSlotSensor == wordChoosenInChars[activeLetterIndex].ToString())
            correctlyPlacedLCCount++;
        else
            correctlyPlacedLCCount--;

        // check if there are any Letter Cubes yet to be placed.
        if (activeLetterIndex == textLength - 1)
        {
            // Check if all letter cubes placed correct or not.
            if (correctlyPlacedLCCount == textLength)
            {
                Debug.Log("Correct word");
                if (words.Count > 0)
                    SpawnLetterCubes(false);
                else
                    Debug.Log("Level Completed");
            }
            else
            {
                Debug.Log("Incorrect word");
                SpawnLetterCubes(true);
            }
        }
        else
        {
            // change the active letter cube as the previous one is placed.
            SetLetterCubeActive(++activeLetterCubeIndex);

        }
    }

    void SetLetterCubeActive(int letterIndex)
    {
        letterCubeMovement.ActiveLetterCube = letterCubesForChoosenWord[letterIndex];
        activeLetterIndex = letterIndex;
    }

}


// public class WordSpawner : MonoBehaviour
// {
//     [SerializeField] int wordLength = 3;
//     [SerializeField] int numberOfWords = 3;
//     [SerializeField] GameObject letterCubeCopy;
//     [SerializeField] GameObject alphabetLetters;
//     [SerializeField] GameObject firstSpawnPoint;
//     [SerializeField] GameObject slotSensors;
//     [SerializeField] GameObject vCam;
//     [SerializeField] GameObject cinemachTargetObject;
//     [SerializeField] float letterCubeScale = 97f;
//     [SerializeField] float distanceBetweenSpawnPoints = 1.5f;
//     [SerializeField] GameDataSave gameDataSave;
//     [SerializeField] WordSubtype wordSubtype = WordSubtype.Word_3;
//     GameObject instantiatedLetterCube;
//     GameObject activeLetterCube;
//     SlotSensorsHandler slotSensorsHandler;

//     List<GameObject> instantiatedLCList;
//     int[] placedLCList;
//     List<Vector3> spawnPoints;
//     List<Word> words;
//     // List<char> userCreatedWord;
//     CinemachineFreeLook cineFreeCam;
//     CinemachineTargetGroup cinemachTargetGroup;

//     int wordIndex;
//     int activeLetterCubeIndex;

//     DatabaseManager databaseManager;


//     // int lastActiveLetterCubeIndex;

//     // Start is called before the first frame update
//     void Start()
//     {
//         // initializing database manager connection
//         databaseManager = new DatabaseManager("wordsDatabase.db");

//         // Objects initializations
//         cineFreeCam = vCam.GetComponent<CinemachineFreeLook>();
//         // cinemachTargetGroup = cinemachTargetObject.GetComponent<CinemachineTargetGroup>();
//         instantiatedLCList = new List<GameObject>(wordLength);
//         spawnPoints = new List<Vector3>(wordLength);
//         // wordChars = new List<char>(wordLength);
//         slotSensorsHandler = slotSensors.GetComponent<SlotSensorsHandler>();
//         // userCreatedWord = new List<char>(wordLength);
//         activeLetterCubeIndex = 0;

//         //subscribing to event
//         // gameDataSave.E_WordCompleted += WordCompleted;
//         gameDataSave.PlaygroundType = PlaygroundType.Words;
//         gameDataSave.WordSubtype = wordSubtype;

//         // gameDataSave.E_PlacedInSlot += OnPlacedInSlot;


//         gameDataSave.WordLength = wordLength;
//         gameDataSave.IsWordCompleted = false;

//         // calculating spawn points
//         CalculateSpawnPoints();


//         // simulating getting words from database
//         // words = TempDatabaseManger.GetWords(wordLength, numberOfWords);
//         words = databaseManager.GetWordsFromDatabase(wordLength, numberOfWords);
//         wordIndex = 0;
//         // GetWordsFromDB();

//         // Instantiating first word
//         InstantiateWord();
//     }

//     private void GetWordsFromDB()
//     {

//         databaseManager.GetWordsFromDatabase(wordLength, numberOfWords);

//     }

//     private void WordCompleted()
//     {
//         // check if the word is correct
//         // Debug.Log("GetUserCreatedWord " + string.Concat(gameDataSave.GetUserCreatedWord()));
//         // Debug.Log("words[wordIndex].Text " + words[wordIndex].Text);
//         if (string.Concat(gameDataSave.GetUserCreatedWord()) == words[wordIndex].Text)
//         {
//             Debug.Log("Correct Word");
//             // gameDataSave.E_PlacedInSlot -= OnPlacedInSlot;
//             wordIndex++;
//             InstantiateWord();
//         }
//         else
//         {
//             Debug.Log("Incorrect Word");
//             // Debug.Log("Press R to retry");
//             // Debug.Log("Press N for next word");
//         }
//         // throw new NotImplementedException();
//     }

//     private void DestroyLetterCubes()
//     {
//         foreach (GameObject item in instantiatedLCList)
//         {
//             Destroy(item);
//         }
//     }



//     void ResetVars()
//     {
//         instantiatedLCList.Clear();
//         instantiatedLetterCube = null;
//         activeLetterCube = null;
//         placedLCList = new int[wordLength];
//         activeLetterCubeIndex = 0;
//         gameDataSave.CurrentWord = words[wordIndex];
//         gameDataSave.IsWordCompleted = false;
//         gameDataSave.InitializeUserCreatedWord();
//     }


//     private void OnPlacedInSlot()
//     {
//         if (!gameDataSave.IsWordCompleted)
//         {
//             // Debug.Log("activeLetterCubeIndex: " + activeLetterCubeIndex);
//             // Debug.Log("instantiatedLCList.Count: " + instantiatedLCList.Count);
//             placedLCList[activeLetterCubeIndex] = 1;

//             // placedLCList.Add(instantiatedLCList[activeLetterCubeIndex]);
//             // instantiatedLCList.RemoveAt(activeLetterCubeIndex);
//             // activeLetterCubeIndex++;
//             // if (activeLetterCubeIndex >= instantiatedLCList.Count)
//             // {
//             //     activeLetterCubeIndex = GetNextActiveElement();
//             // }

//             // activeLetterCubeIndex = GetNextActiveElement(activeLetterCubeIndex);
//             setNextPendingLCIndex();
//             activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot -= OnPlacedInSlot;
//             ActivateLetterCube();
//         }
//         else
//         {
//             WordCompleted();
//         }

//     }

//     private void setNextPendingLCIndex()
//     {
//         // Debug.Log("startFromIndex: "+startFromIndex);
//         // int arrayTraversed = 0;

//         // start after active cube index, set and return if found else stop.

//         for (int i = activeLetterCubeIndex + 1; i < wordLength; i++)
//         {
//             if (placedLCList[i] == 0)
//             {
//                 activeLetterCubeIndex = i;
//                 return;
//             }
//         }
//         // no pendig LC after active LC, so start from 0 and go till active LC
//         for (int i = 0; i < activeLetterCubeIndex; i++)
//         {
//             if (placedLCList[i] == 0)
//             {
//                 activeLetterCubeIndex = i;
//                 return;
//             }
//         }
//     }



//     private void DeactivateLetterCube()
//     {

//         if (activeLetterCube != null && activeLetterCube.TryGetComponent<LetterCubeMovement>(out LetterCubeMovement letterCubeMovement))
//         {
//             letterCubeMovement.enabled = false;
//         }
//     }

//     private void ActivateLetterCube()
//     {
//         // Debug.Log("calledFrom: "+calledFrom);
//         DeactivateLetterCube();
//         // Debug.Log("activeLetterCubeIndex: "+activeLetterCubeIndex );

//         activeLetterCube = instantiatedLCList[activeLetterCubeIndex];
//         if (activeLetterCube.TryGetComponent<LetterCubeMovement>(out LetterCubeMovement letterCubeMovement))
//         {

//             activeLetterCube.GetComponent<LetterCubeMovement>().enabled = true;
//             cineFreeCam.Follow = activeLetterCube.transform;
//             cineFreeCam.LookAt = activeLetterCube.transform;
//         }
//         else
//         {

//             Debug.Log($"Letter Cube at index {activeLetterCubeIndex + 1} is not active");
//             Debug.Log($"Press Q to switch to the next Letter Cube");
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         ProcessInput();
//     }

//     private void ProcessInput()
//     {
//         // for (int i = 0; i < 5; i++)
//         // {
//         //     if (Input.GetKeyDown((KeyCode)(int)KeyCode.Alpha1 + i) || Input.GetKeyDown((KeyCode)(int)KeyCode.Keypad1 + i))
//         //     {
//         //         if (i < instantiatedLCList.Count)
//         //         {
//         //             activeLetterCubeIndex = GetNextActiveElement(i);
//         //             Debug.Log("activeLetterCubeIndex(Key): "+activeLetterCubeIndex);
//         //             // activeLetterCubeIndex = i;

//         //             ActivateLetterCube();
//         //         }
//         //         else
//         //         {
//         //             Debug.Log($"No Letter Cube at index {i + 1}");
//         //         }
//         //         return;
//         //     }
//         // }
//         if (Input.GetKeyDown(KeyCode.Q))
//         {
//             // activeLetterCubeIndex++;
//             setNextPendingLCIndex();
//             Debug.Log("activeLetterCubeIndex(Q): " + activeLetterCubeIndex);
//             // if (activeLetterCubeIndex >= instantiatedLCList.Count)
//             // {
//             //     activeLetterCubeIndex = GetNextActiveElement();
//             // }
//             ActivateLetterCube();
//         }
//         if (Input.GetKeyDown(KeyCode.R))
//         {
//             // gameDataSave.E_PlacedInSlot -= OnPlacedInSlot;
//             InstantiateWord();
//         }
//         if (Input.GetKeyDown(KeyCode.N))
//         {
//             // gameDataSave.E_PlacedInSlot -= OnPlacedInSlot;
//             wordIndex++;
//             InstantiateWord();
//         }
//     }
// }
