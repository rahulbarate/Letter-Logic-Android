using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSpawner : Spawner
{
    List<int> availableNumbers;
    [SerializeField] GameObject numberLetterCubes;
    [SerializeField] GameObject gameWonPanel;
    private int consecutiveCorrect;
    private int threshold = 4;
    PowerUpManager powerUpManager;
    // Start is called before the first frame update
    void Start()
    {
        GenerateAllLetters();
        powerUpManager = GetComponent<PowerUpManager>();
        currentHealth = maxHealth;
        consecutiveCorrect = 0;
        letterCubeMovement = GetComponent<LetterCubeMovement>();
        slotSensorsHandler.AssignENumbersToSlotSensors();
        // InstantiateLetterCube();
        SpawnLetterCubes();
        Time.timeScale = 1f;
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

    void SpawnLetterCubes()
    {
        if (availableNumbers.Count != 0)
        {
            // generating random index and calculating letter cube to fetch from 26 letter cubes
            int randomNumberIndex = UnityEngine.Random.Range(0, availableNumbers.Count);
            int numberToFetch = availableNumbers[randomNumberIndex] - 1;

            //getting letter string
            letterChoosen = availableNumbers[randomNumberIndex].ToString();

            correctSlotSensorIndex.Clear();
            correctSlotSensorIndex.Add(numberToFetch);

            activeLetterCube = numberLetterCubes.transform.GetChild(numberToFetch).gameObject;

            activeLetterCube.transform.localScale = new UnityEngine.Vector3(letterCubeScale, letterCubeScale, letterCubeScale);

            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot += OnPlacedInSlot;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeBombed += OnLetterCubeBombed;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeFell += OnLetterCubeFell;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PickedPowerUp += powerUpManager.OnPowerUpPickedUp;

            activeLetterCube.GetComponent<LetterCubeData>().LetterOnTop = letterChoosen;

            activeLetterCube.transform.position = transform.position;

            activeLetterCube.GetComponent<Rigidbody>().isKinematic = false;
            activeLetterCube.GetComponent<Rigidbody>().useGravity = true;

            availableNumbers.RemoveAt(randomNumberIndex);

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
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PickedPowerUp -= powerUpManager.OnPowerUpPickedUp;

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

            // InstantiateLetterCube();
            SpawnLetterCubes();
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
        healthText.text = currentHealth.ToString();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
