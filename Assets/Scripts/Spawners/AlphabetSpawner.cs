using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetSpawner : Spawner
{
    List<char> availableLetters;
    [SerializeField] GameObject alphabetLetterCubes;
    [SerializeField] GameObject gameWonPanel;
    // [SerializeField] HintMechanism hintMechanism;
    [SerializeField] char startChar = 'A';
    [SerializeField] char endChar = 'Z';

    private int consecutiveCorrect;
    private int threshold = 5;

    // Start is called before the first frame update
    void Start()
    {
        GenerateAllLetters();
        letterCubeMovement = GetComponent<LetterCubeMovement>();
        currentHealth = maxHealth;
        healthBarSegments = maxHealth;
        consecutiveCorrect = 0;
        slotSensorsHandler.AssignCLettersToSlotSensors();
        SpawnLetterCubes();
        Time.timeScale = 1f;
        // InstantiateLetterCube();
    }

    // Update is called once per frame
    void Update()
    {
        // if (currentHealth <= 0)
        // {
        //     Time.timeScale = 0f;
        //     gameOverPanel.SetActive(true);
        // }
    }
    private void GenerateAllLetters()
    {
        availableLetters = new List<char>();

        for (char i = startChar; i <= endChar; i++)
            availableLetters.Add(i);
    }
    void SpawnLetterCubes()
    {
        if (availableLetters.Count != 0)
        {
            // generating random index and calculating letter cube to fetch from 26 letter cubes
            int randomLetterIndex = UnityEngine.Random.Range(0, availableLetters.Count);
            int letterCubeToFetch = 26 - (90 - Convert.ToInt32(availableLetters[randomLetterIndex])) - 1;

            correctSlotSensorIndex.Clear();
            correctSlotSensorIndex.Add(letterCubeToFetch);

            //getting letter string
            letterChoosen = availableLetters[randomLetterIndex].ToString();

            activeLetterCube = alphabetLetterCubes.transform.GetChild(letterCubeToFetch).gameObject;

            // activeLetterCube.transform.localScale = new UnityEngine.Vector3(0.93f, 0.93f, 0.93f);
            // Debug.Log(letterCubeScale);
            activeLetterCube.transform.localScale = new Vector3(letterCubeScale, letterCubeScale, letterCubeScale);

            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot += OnPlacedInSlot;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeBombed += OnLetterCubeBombed;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeFell += OnLetterCubeFell;

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
            // hintMechanism.AddHint(2, false);
            // gameDataSave.IsLevelCompleted = true;
            // gameDataSave.LetterCube = null;
            // Time.timeScale = 0f;
            gameWonPanel.SetActive(true);
        }
    }

    public override void OnPlacedInSlot(string letterOnSlotSensor)
    {
        if (letterOnSlotSensor == letterChoosen)
        {
            // update milstone
            milestoneManager.HandleCubePlaced();

            // making sure no forces are applied
            activeLetterCube.GetComponent<Rigidbody>().isKinematic = true;
            activeLetterCube.GetComponent<Rigidbody>().useGravity = false;

            // remove event listening.
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot -= OnPlacedInSlot;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeBombed -= OnLetterCubeBombed;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeFell -= OnLetterCubeFell;

            // setting isPlaced to true, so bombs won't affect it.
            activeLetterCube.GetComponent<LetterCubeData>().isPlaced = true;

            //Reset vars
            single3DLetterModel = null;
            activeLetterCube = null;
            letterChoosen = null;
            activeLetterCubeEventHandler = null;
            letterCubeMovement.ActiveLetterCube = null;

            // consecutiveCorrect++;
            // if (consecutiveCorrect >= threshold)
            // {
            //     hintMechanism.AddHint(1);
            //     consecutiveCorrect = 0;
            // }

            SpawnLetterCubes();

            // InstantiateLetterCube();
        }
        else
        {
            // Process incorrect Letter Cube placement
            // Debug.Log("Incorrect Letter Cube");
            TakeDamage();

            // update milestone
            milestoneManager.HandleDamageTaken();

            // activeLetterCubeEventHandler.ProcessIncorrectLetterCube();
            letterCubeMovement.MoveToInitialPosition();
            consecutiveCorrect = 0;
        }
    }

    public override void OnLetterCubeBombed(GameObject letterCubeHit)
    {
        letterCubeMovement.MoveToInitialPosition();
        TakeDamage();
        // update milestone
        milestoneManager.HandleDamageTaken();
    }
    public override void OnLetterCubeFell(GameObject letterCubeHit)
    {
        letterCubeMovement.MoveToInitialPosition();
        TakeDamage();
        // update milestone
        milestoneManager.HandleDamageTaken();
    }
    public override void ReviveLevel()
    {
        currentHealth = maxHealth;
        healthBarSegments = maxHealth;
        consecutiveCorrect = 0;
        if (healthBar != null)
        {
            healthBar.SetActive(true);
            for (int i = 0; i < healthBar.transform.childCount; i++)
            {
                healthBar.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
