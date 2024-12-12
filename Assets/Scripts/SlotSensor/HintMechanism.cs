using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintMechanism : MonoBehaviour
{
    [SerializeField] public int availableHints = 30;
    [SerializeField] GameObject requestPlatform;
    [SerializeField] GameDataSave gameDataSave;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ProcessHint();
        }
    }

    private void ProcessHint()
    {
        // LetterCubeData letterCubeData = requestPlatform.GetComponent<AlphabetLCInstantiator>().GetCurrentLetterCubeData();
        GameObject letterCube = gameDataSave.LetterCube;
        if (availableHints <= 0)
        {
            Debug.Log("No hints available!");
            return;
        }
        // if (letterCubeData && letterCubeData.GetLetterCubeState() == LetterCubeState.Matched)
        // {
        //     return;
        // }

        // int slotSensorToHighlight = 26 - (90 - Convert.ToInt32(letterCubeData.GetLetterOnCube())) - 1;

        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf == true)
            {
                availableHints--;
                child.GetComponent<Light>().enabled = true;
                return;
            }
            // SlotSensorData slotSensorData = child.GetComponent<SlotSensorData>();
            // Light slotSensorLight = child.GetComponent<Light>();
            // if (slotSensorData.Letter == letterCube.GetComponent<LetterCubeData>().GetLetterOnCube() && !slotSensorLight.enabled)
            // {

            //     availableHints--;
            //     slotSensorLight.enabled = true;
            //     Debug.Log("Look for illuminated slot");
            //     return;

            // }
        }
        // transform.GetChild(slotSensorToHighlight).GetComponent<Light>().enabled = true;

    }
}
