using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberSpawner : Spawner
{
    List<int> availableNumbers;
    [SerializeField] GameObject numberLetterCubes;
    [SerializeField] GameObject gameWonPanel;
    [SerializeField] GameObject gameWonAdPanel;
    [SerializeField] ParticleSystem gameWonParticleEffect;
    [SerializeField] TextMeshProUGUI gameWonPanelRewardText;
    [SerializeField] int gameWonRewardCoins = 200;
    private int consecutiveCorrect;
    private int threshold = 4;
    PowerUpManager powerUpManager;
    // Start is called before the first frame update
    void Start()
    {
        GenerateAllLetters();
        powerUpManager = GetComponent<PowerUpManager>();
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
        consecutiveCorrect = 0;
        letterCubeMovement = GetComponent<LetterCubeMovement>();
        slotSensorsHandler.AssignENumbersToSlotSensors();
        // InstantiateLetterCube();
        SpawnLetterCubes();
        Time.timeScale = 1f;
        // gameDataSave.NoOfTimesGameOver = 0;
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
            AdService.E_RewardedAdCompleted -= OnRewardedAdCompleted;
            AdService.E_RewardedAdCompleted += OnRewardedAdCompleted;

            isLevelWon = false;
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
            powerUpManager.ToggleSwirlEffect();
        }
        else
        {
            Debug.Log("Level Completed");
            isLevelWon = true;

            // reward user with coins
            gameDataSave.TotalAvailableCoins += gameWonRewardCoins;

            gameWonPanelRewardText.text = $"Congratulations, you have been rewarded with {gameWonRewardCoins}C";
            // display get 2x reward ad panel if ad is available.
            if (AdService.Instance != null && AdService.Instance.IsRewardedAdReady())
                gameWonAdPanel.SetActive(true);
            else
                gameWonAdPanel.SetActive(false);

            gameWonPanel.SetActive(true);
            gameWonParticleEffect.Play();
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

            // disabling hints
            powerUpManager.DisableHintPowerUp();

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
    public override void GetDoubleReward()
    {
        gameDataSave.TotalAvailableCoins += gameWonRewardCoins;
        gameWonPanelRewardText.text = $"Hurray your reward is doubled! total coins earned {gameWonRewardCoins * 2}";
    }
}
