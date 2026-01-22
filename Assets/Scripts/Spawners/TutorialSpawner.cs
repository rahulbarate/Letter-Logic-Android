using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(TutorialSequencer))]
public class TutorialSpawner : Spawner
{
    [SerializeField] char[] lettersToSpawn;
    List<char> availableLetters;
    [SerializeField] GameObject alphabetLetterCubes;
    [SerializeField] GameObject gameWonPanel;
    [SerializeField] GameObject gameWonAdPanel;
    [SerializeField] ParticleSystem gameWonParticleEffect;
    [SerializeField] TextMeshProUGUI gameWonPanelRewardText;
    [SerializeField] int gameWonRewardCoins = 200;
    [SerializeField] char startChar = 'A';
    [SerializeField] char endChar = 'Z';

    private int consecutiveCorrect;
    private int threshold = 5;

    PowerUpManager powerUpManager;
    TutorialSequencer tutorialSequencer;

    // Start is called before the first frame update
    void Start()
    {
        tutorialSequencer = GetComponent<TutorialSequencer>();
        // GenerateAllLetters();
        availableLetters = new List<char>(lettersToSpawn);
        // CustomLogger.Log(availableLetters.Count);
        letterCubeMovement = GetComponent<LetterCubeMovement>();
        powerUpManager = GetComponent<PowerUpManager>();
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
        // healthBarSegments = maxHealth;
        consecutiveCorrect = 0;
        slotSensorsHandler.AssignCLettersToSlotSensors();
        SpawnLetterCubes();
        Time.timeScale = 1f;
        // gameDataSave.NoOfTimesGameOver = 0;
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
            AdService.E_RewardedAdCompleted -= OnRewardedAdCompleted;
            AdService.E_RewardedAdCompleted += OnRewardedAdCompleted;

            isLevelWon = false;

            // generating random index and calculating letter cube to fetch from 26 letter cubes
            // int randomLetterIndex = UnityEngine.Random.Range(0, availableLetters.Count);

            // int letterIndex = availableLetters[0];
            int letterCubeToFetch = 26 - (90 - Convert.ToInt32(availableLetters[0])) - 1;

            correctSlotSensorIndex.Clear();
            correctSlotSensorIndex.Add(letterCubeToFetch);

            //getting letter string
            letterChoosen = availableLetters[0].ToString();
            if (letterChoosen == "D")
            {
                tutorialSequencer.StartPowerupSequence();
            }

            activeLetterCube = alphabetLetterCubes.transform.GetChild(letterCubeToFetch).gameObject;

            // activeLetterCube.transform.localScale = new UnityEngine.Vector3(0.93f, 0.93f, 0.93f);
            // Debug.Log(letterCubeScale);
            activeLetterCube.transform.localScale = new Vector3(letterCubeScale, letterCubeScale, letterCubeScale);

            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot += OnPlacedInSlot;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeBombed += OnLetterCubeBombed;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PickedPowerUp += powerUpManager.ActivatePowerUp;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeFell += OnLetterCubeFell;

            activeLetterCube.GetComponent<LetterCubeData>().LetterOnTop = letterChoosen;

            activeLetterCube.transform.position = transform.position;

            activeLetterCube.GetComponent<Rigidbody>().isKinematic = false;
            activeLetterCube.GetComponent<Rigidbody>().useGravity = true;

            availableLetters.RemoveAt(0);

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
            AudioManager.instance.PlayGameWonSFX();
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
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PickedPowerUp -= powerUpManager.ActivatePowerUp;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeFell -= OnLetterCubeFell;

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

            if (letterOnSlotSensor == "D")
                tutorialSequencer.EndTutorial();
            else
                SpawnLetterCubes();

            // InstantiateLetterCube();
        }
        else
        {
            // Process incorrect Letter Cube placement
            // Debug.Log("Incorrect Letter Cube");
            // TakeDamage();

            // update milestone
            // milestoneManager.HandleDamageTaken();

            // activeLetterCubeEventHandler.ProcessIncorrectLetterCube();
            letterCubeMovement.MoveToInitialPosition();
            consecutiveCorrect = 0;
        }
    }

    public override void OnLetterCubeBombed(GameObject letterCubeHit)
    {
        cameraEffect.ShakeCamera();
        milestoneManager.HandleDamageTaken();
        letterCubeMovement.disableMovement = true;
        lCDebris.transform.position = activeLetterCube.transform.position + Vector3.up * 1f;
        lCDebris.SetActive(true);
        activeLetterCube.SetActive(false);
        Invoke(nameof(AfterBombedSequence), afterBombedSequenceDelay);

    }
    void AfterBombedSequence()
    {
        TakeDamage();
        letterCubeMovement.MoveToInitialPosition();
        letterCubeMovement.disableMovement = false;
        activeLetterCube.SetActive(true);
        lCDebris.SetActive(false);

        foreach (Transform child in lCDebris.transform)
            child.localPosition = Vector3.zero;
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
        // AdService.E_RewardedAdCompleted -= ReviveLevel;
    }
    public override void GetDoubleReward()
    {
        gameDataSave.TotalAvailableCoins += gameWonRewardCoins;
        gameWonPanelRewardText.text = $"Hurray your reward is doubled! total coins earned {gameWonRewardCoins * 2}";
    }
}
