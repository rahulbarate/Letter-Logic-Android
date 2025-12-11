using System;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using UnityEngine;

public class WordSpawner : Spawner
{
    [SerializeField] int textLength = 3;
    [SerializeField] int numberOfWords = 3;
    [SerializeField] GameObject alphabetLetterCubes;
    [SerializeField] GameObject firstSpawnPoint;
    [SerializeField] float distanceBetweenSpawnPoints = 1.5f;
    [SerializeField] GameObject tempGameWonPanel;
    [SerializeField] GameObject correctWordPanel;
    [SerializeField] GameObject correctWordPanelAdButton;
    [SerializeField] GameObject incorrectWordPanel;

    [SerializeField] ToastUI toastUI;

    List<UnityEngine.Vector3> spawnPoints;
    List<Word> words;
    DatabaseManager databaseManager;
    int activeLetterCubeIndex;
    Word wordChosen;
    List<char> wordChosenInChars;
    List<GameObject> letterCubesForChosenWord;
    List<GameObject> instantiatedLetterCubes;

    int correctlyPlacedLCCount = 0;
    private int consecutiveWins = 0;
    private int totalWins = 0;
    [SerializeField]
    private int consecutiveThreshold = 3;
    [SerializeField]
    private int totalThreshold = 10;
    PowerUpManager powerUpManager;


    void Start()
    {
        powerUpManager = GetComponent<PowerUpManager>();
        currentHealth = maxHealth;
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
        Time.timeScale = 1f;

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
            Time.timeScale = 0f;
            tempGameWonPanel.SetActive(true);
            return;
        }
        wordChosen = words[0];
        words.RemoveAt(0);

        // split it into characters
        wordChosenInChars = new List<char>(wordChosen.Text.ToCharArray());

        //Display Hint
        CustomLogger.Log("Hint:" + wordChosen.Hint);
        ShowTextualHint();
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

                letterCube.transform.localScale = new UnityEngine.Vector3(letterCubeScale * 100f, letterCubeScale * 100f, letterCubeScale * 100f);
            }
            else
                letterCube.transform.localScale = new UnityEngine.Vector3(letterCubeScale, letterCubeScale, letterCubeScale);


            // Register to events
            RegisterEvents(letterCube);

            letterCube.GetComponent<LetterCubeData>().LetterOnTop = wordChosenInChars[randomCharIndex].ToString();
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

    private void RegisterEvents(GameObject letterCube)
    {
        letterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot += OnPlacedInSlot;
        letterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeBombed += OnLetterCubeBombed;
        letterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeFell += OnLetterCubeFell;
        letterCube.GetComponent<LetterCubeEventHandler>().E_PickedPowerUp += powerUpManager.OnPowerUpPickedUp;
    }
    private void UnregisterEvents(GameObject letterCube)
    {
        letterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot -= OnPlacedInSlot;
        letterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeBombed -= OnLetterCubeBombed;
        letterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeFell -= OnLetterCubeFell;
        letterCube.GetComponent<LetterCubeEventHandler>().E_PickedPowerUp -= powerUpManager.OnPowerUpPickedUp;
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

            // unsubscribe to events
            UnregisterEvents(letterCubesForChosenWord[i]);
            // re register the event
            RegisterEvents(letterCubesForChosenWord[i]);
            // letterCubesForChosenWord[i].GetComponent<LetterCubeEventHandler>().E_PlacedInSlot -= OnPlacedInSlot;
            // letterCubesForChosenWord[i].GetComponent<LetterCubeEventHandler>().E_PlacedInSlot += OnPlacedInSlot;
            // letterCubesForChosenWord[i].GetComponent<LetterCubeEventHandler>().E_LetterCubeBombed -= OnLetterCubeBombed;
            // letterCubesForChosenWord[i].GetComponent<LetterCubeEventHandler>().E_LetterCubeBombed += OnLetterCubeBombed;
            // letterCubesForChosenWord[i].GetComponent<LetterCubeEventHandler>().E_LetterCubeFell -= OnLetterCubeFell;
            // letterCubesForChosenWord[i].GetComponent<LetterCubeEventHandler>().E_LetterCubeFell -= OnLetterCubeFell;
            // letterCubesForChosenWord[i].GetComponent<LetterCubeEventHandler>().E_PickedPowerUp -= powerUpManager.OnPowerUpPickedUp;
            // letterCubesForChosenWord[i].GetComponent<LetterCubeEventHandler>().E_PickedPowerUp += powerUpManager.OnPowerUpPickedUp;
            // set rigidbody is kinematic to true, so cube isn't moveable if it is not active.
            letterCubesForChosenWord[i].GetComponent<Rigidbody>().isKinematic = true;
            // disable the gravity
            letterCubesForChosenWord[i].GetComponent<Rigidbody>().useGravity = false;



        }

        // Un-hide the Letter Cubes
        ToggleLetterCubesVisibility(true);

        //Set the Letter Cube Active
        SetLetterCubeActive(0);

        ShowTextualHint();
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

    public override void OnPlacedInSlot(string letterOnSlotSensor)
    {
        // CustomLogger.Log($"Cube placed; letterOnSlotSensor: {letterOnSlotSensor}, letterOnCube: {activeLetterCube.GetComponent<LetterCubeData>().GetLetterOnCube()}");

        // setting isPlaced to true so they won't be affected by bombing.
        activeLetterCube.GetComponent<LetterCubeData>().isPlaced = true;
        UnregisterEvents(activeLetterCube);

        activeLetterCube.GetComponent<Rigidbody>().isKinematic = true;
        activeLetterCube.GetComponent<Rigidbody>().useGravity = false;
        letterCubeMovement.ActiveLetterCube = null;

        // for (int i = 0; i <= textLength - 1; i++)
        // {
        //     Debug.Log(wordChosenInChars[activeLetterCubeIndex].ToString());
        // }
        if (letterOnSlotSensor == activeLetterCube.GetComponent<LetterCubeData>().LetterOnTop)
        {
            // Debug.Log($"Correctly placed; letterOnSlotSensor:{letterOnSlotSensor} == {activeLetterCube.GetComponent<LetterCubeData>().GetLetterOnCube()}");
            correctlyPlacedLCCount++;

            // update milestone
            // milestoneManager.HandleCubePlaced();
        }
        else
        {
            // Debug.Log($"In-Correctly placed; letterOnSlotSensor:{letterOnSlotSensor} != {activeLetterCube.GetComponent<LetterCubeData>().GetLetterOnCube()}");
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

                // update milstone
                milestoneManager.HandleWordCompleted();

                totalWins += 1;
                consecutiveWins += 1;

                if (words.Count == 0)
                {
                    CustomLogger.Log("No more words to spawn.");
                    Time.timeScale = 0f;
                    tempGameWonPanel.SetActive(true);
                    return;
                }
                else
                {

                    correctWordPanel.SetActive(true);

                    // if (consecutiveWins >= consecutiveThreshold)
                    // {
                    //     CustomLogger.Log($"Consec won {totalWins}, {consecutiveWins}");
                    //     hintMechanism.AddHint(1, false);// don't show toast
                    //     correctWordPanelAdButton.SetActive(true);
                    //     consecutiveWins = 0;
                    // }

                    // if (totalWins >= totalThreshold)
                    // {
                    //     CustomLogger.Log($"Total won {totalWins}, {consecutiveWins}");
                    //     hintMechanism.AddHint(2, false);
                    //     correctWordPanelAdButton.SetActive(true);
                    //     totalWins = 0;
                    //     consecutiveWins = 0;
                    // }
                }

            }
            else
            {
                CustomLogger.LogWarning("Incorrect word");

                // update milestone
                milestoneManager.HandleIncorrectWord();

                consecutiveWins = 0;
                // Time.timeScale = 0f;
                incorrectWordPanel.SetActive(true);
                // RespawnLetterCubes();
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
        correctSlotSensorIndex.Clear();
        activeLetterCube = letterCubesForChosenWord[letterIndex];
        // CustomLogger.Log(activeLetterCube.GetComponent<LetterCubeData>().LetterOnTop[0]);
        int firstOccurance = wordChosen.Text.IndexOf(activeLetterCube.GetComponent<LetterCubeData>().LetterOnTop[0]);
        correctSlotSensorIndex.Add(firstOccurance);
        int lastOccurance = wordChosen.Text.LastIndexOf(activeLetterCube.GetComponent<LetterCubeData>().LetterOnTop[0]);
        if (firstOccurance != lastOccurance)
            correctSlotSensorIndex.Add(lastOccurance);
        letterCubeMovement.ActiveLetterCube = activeLetterCube;
        activeLetterCube.GetComponent<Rigidbody>().isKinematic = false;
        activeLetterCube.GetComponent<Rigidbody>().useGravity = true;
        // activeLetterCubeIndex = letterIndex;
        cineFreeCam.Follow = letterCubesForChosenWord[letterIndex].transform;
        cineFreeCam.LookAt = letterCubesForChosenWord[letterIndex].transform;
    }
    public override void OnLetterCubeBombed(GameObject letterCubeHit)
    {
        if (activeLetterCube.gameObject == letterCubeHit)
        {
            CustomLogger.Log("Bombed");
            letterCubeMovement.MoveToInitialPosition();
            TakeDamage();

            // update milestone
            milestoneManager.HandleDamageTaken();
        }
    }
    public override void OnLetterCubeFell(GameObject letterCubeHit)
    {
        if (activeLetterCube.gameObject == letterCubeHit)
        {
            CustomLogger.Log("Fell in ocean");
            letterCubeMovement.MoveToInitialPosition();
            TakeDamage();

            // update milestone
            milestoneManager.HandleDamageTaken();
        }
    }
    public override void ReviveLevel()
    {
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
        if (incorrectWordPanel != null) incorrectWordPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        RespawnLetterCubes();
        Time.timeScale = 1f;
    }

    public void SpawnNextWord()
    {
        SpawnLetterCubes();
    }

    public void ShowTextualHint()
    {
        if (wordChosen != null && toastUI != null)
        {
            toastUI.ShowToast($"{wordChosen.Hint}");
        }
    }

}