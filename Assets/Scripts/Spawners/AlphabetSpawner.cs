using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetSpawner : Spawner
{
    List<char> availableLetters;
    [SerializeField] GameObject alphabetLetterCubes;
    [SerializeField] GameObject gameWonPanel;

    // Start is called before the first frame update
    void Start()
    {
        GenerateAllLetters();
        letterCubeMovement = GetComponent<LetterCubeMovement>();
        currentHealth = maxHealth;
        healthBarSegments = maxHealth;
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

            correctSlotSensorIndex.Clear();
            correctSlotSensorIndex.Add(letterCubeToFetch);

            //getting letter string
            letterChoosen = availableLetters[randomLetterIndex].ToString();

            activeLetterCube = alphabetLetterCubes.transform.GetChild(letterCubeToFetch).gameObject;

            activeLetterCube.transform.localScale = new UnityEngine.Vector3(0.95f, 0.95f, 0.95f);

            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_PlacedInSlot += OnPlacedInSlot;
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeBombed += OnLetterCubeBombed;

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
            // gameDataSave.IsLevelCompleted = true;
            // gameDataSave.LetterCube = null;
            Time.timeScale = 0f;
            gameWonPanel.SetActive(true);
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
            activeLetterCube.GetComponent<LetterCubeEventHandler>().E_LetterCubeBombed -= OnLetterCubeBombed;

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
            // Debug.Log("Incorrect Letter Cube");
            TakeDamage();
            // activeLetterCubeEventHandler.ProcessIncorrectLetterCube();
            letterCubeMovement.MoveToInitialPosition();
        }
    }

    public override void OnLetterCubeBombed(GameObject letterCubeHit)
    {
        letterCubeMovement.MoveToInitialPosition();
        TakeDamage();
    }

    public override void ReviveLevel()
    {
        currentHealth = maxHealth;
        healthBarSegments = maxHealth;
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
