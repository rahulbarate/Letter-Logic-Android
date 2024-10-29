using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LetterCubesInstantiator : MonoBehaviour
{
    [SerializeField] GameObject allLetters;
    [SerializeField] GameObject letterCubeCopy;
    [SerializeField] GameObject vCam;
    [SerializeField] float letterCubeScale = 97f;
    GameObject letterCopy;
    GameObject lastInstantiatedLetterCube;
    LetterCubeData lastInstantiatedLetterCubeData;

    List<char> availableLetters;

    CinemachineFreeLook cineFreeCam;


    // Start is called before the first frame update
    void Start()
    {
        cineFreeCam = vCam.GetComponent<CinemachineFreeLook>();
        GenerateAllLetters();
        InstantiateLetterCube();
    }
    private void GenerateAllLetters()
    {
        availableLetters = new List<char>();
        for (char i = 'A'; i <= 'Z'; i++)
        {
            availableLetters.Add(i);
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
            lastInstantiatedLetterCube = Instantiate(letterCubeCopy, transform.position, Quaternion.identity);
            lastInstantiatedLetterCube.transform.localScale = new Vector3(letterCubeScale, letterCubeScale, letterCubeScale);

            //checking if letter cube has data script, else adding it.
            if (lastInstantiatedLetterCube.GetComponent<LetterCubeData>() == null)
            {
                lastInstantiatedLetterCube.AddComponent<LetterCubeData>();
            }
            lastInstantiatedLetterCubeData = lastInstantiatedLetterCube.GetComponent<LetterCubeData>();

            // setting letter to be displayed on top and storing it in data script.
            lastInstantiatedLetterCubeData.SetLetterOnCube(availableLetters[randomLetterIndex], letterCopy);
            availableLetters.RemoveAt(randomLetterIndex);

            // setting camera to follow newly created Letter Cube.
            cineFreeCam.Follow = lastInstantiatedLetterCube.transform;
            cineFreeCam.LookAt = lastInstantiatedLetterCube.transform;




        }
        else
        {
            Debug.Log("Level Completed");
        }

    }



    // Update is called once per frame
    void Update()
    {
        // checking if Letter Cube state is changed.
        if (lastInstantiatedLetterCubeData.GetLetterCubeState() == LetterCubeState.Matched)
        {
            lastInstantiatedLetterCubeData.ProcessCorrectLetterCube();
            InstantiateLetterCube();
        }
    }
}
