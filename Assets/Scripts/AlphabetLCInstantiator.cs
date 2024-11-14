using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AlphabetLCInstantiator : MonoBehaviour
{
    [SerializeField] GameObject allLetters;
    [SerializeField] GameObject letterCubeCopy;
    [SerializeField] GameObject invisibleCharacter;
    [SerializeField] GameObject vCam;
    [SerializeField] float letterCubeScale = 97f;
    [SerializeField] AlphabetSubtype alphabetSubtype = AlphabetSubtype.C_Letters;
    GameObject letterCopy;
    GameObject newLetterCube;
    LetterCubeData newLCData;
    LetterCubeHandler newLCHandler;

    List<char> availableLetters;

    CinemachineFreeLook cineFreeCam;
    // bool isLevelCompleted;

    // public bool IsLevelCompleted
    // {
    //     get { return isLevelCompleted; }
    //     set { isLevelCompleted = value; }
    // }


    // Start is called before the first frame update
    void Start()
    {
        GameDataSave.IsLevelCompleted = false;
        cineFreeCam = vCam.GetComponent<CinemachineFreeLook>();
        GameDataSave.PlaygroundType = PlaygroundType.Alphabet;
        GameDataSave.AlphabetSubtype = alphabetSubtype;
        GenerateAllLetters();
        InstantiateLetterCube();
    }
    private void GenerateAllLetters()
    {
        availableLetters = new List<char>();
        if (alphabetSubtype == AlphabetSubtype.C_Letters)
        {
            for (char i = 'A'; i <= 'D'; i++)
            {
                availableLetters.Add(i);
            }

        }
        else if (alphabetSubtype == AlphabetSubtype.S_Letters)
        {
            for (char i = 'a'; i <= 'z'; i++)
            {
                availableLetters.Add(i);
            }
        }

    }

    private void InstantiateLetterCube()
    {
        if (availableLetters.Count != 0)
        {
            // generating random index and calculating letter to fetch from 26 lettes
            int randomLetterIndex = UnityEngine.Random.Range(0, availableLetters.Count);
            int letterToFetch = 26 - (90 - Convert.ToInt32(availableLetters[randomLetterIndex])) - 1;

            //fetching letter object to be displayed on the Cube.
            letterCopy = allLetters.transform.GetChild(letterToFetch).gameObject;


            // Instantiating letter and setting scale.
            newLetterCube = Instantiate(letterCubeCopy, transform.position, Quaternion.identity);
            newLetterCube.transform.localScale = new Vector3(letterCubeScale, letterCubeScale, letterCubeScale);

            //checking if letter cube has data script, else adding it.
            if (newLetterCube.GetComponent<LetterCubeData>() == null)
            {
                newLetterCube.AddComponent<LetterCubeData>();
            }
            newLCData = newLetterCube.GetComponent<LetterCubeData>();

            //checking if letter cube has handler script, else adding it.
            if (newLetterCube.GetComponent<LetterCubeHandler>() == null)
            {
                newLetterCube.AddComponent<LetterCubeHandler>();
            }
            newLCHandler = newLetterCube.GetComponent<LetterCubeHandler>();




            // setting letter to be displayed on top and storing it in data script.
            newLCData.SetLetterOnCube(availableLetters[randomLetterIndex], letterCopy);
            availableLetters.RemoveAt(randomLetterIndex);
            GameDataSave.LetterCubeData = newLCData;

            // setting camera to follow newly created Letter Cube.
            cineFreeCam.Follow = newLetterCube.transform;
            cineFreeCam.LookAt = newLetterCube.transform;




        }
        else
        {
            Debug.Log("Level Completed");
            GameDataSave.IsLevelCompleted = true;
            GameDataSave.LetterCubeData = null;
            cineFreeCam.Follow = invisibleCharacter.transform;
            cineFreeCam.LookAt = invisibleCharacter.transform;
            invisibleCharacter.SetActive(true);

        }

    }

    public LetterCubeData GetCurrentLetterCubeData()
    {
        return newLCData;

    }



    // Update is called once per frame
    void Update()
    {

        if (!GameDataSave.IsLevelCompleted)
        {
            LetterCubeState newLCState = newLCData.GetLetterCubeState();

            // checking if Letter Cube state is changed.
            if (newLCState == LetterCubeState.Matched)
            {
                newLCHandler.ProcessCorrectLetterCube();
                InstantiateLetterCube();
            }
            else if (newLCState == LetterCubeState.Mismatched)
            {
                newLCHandler.ProcessIncorrectLetterCube(transform.localPosition);
            }
            else if (newLCState == LetterCubeState.Bombed)
            {
                newLCHandler.ProcessBombedLetterCube(transform.localPosition);
            }
        }
    }
}
