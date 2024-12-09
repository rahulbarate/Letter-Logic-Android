using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintMechanism : MonoBehaviour
{
    [SerializeField] public int availableHints = 30;
    [SerializeField] GameObject requestPlatform;

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
        LetterCubeData letterCubeData = GameDataSave.LetterCubeData;
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
            SlotSensorData slotSensorData = child.GetComponent<SlotSensorData>();
            Light slotSensorLight = child.GetComponent<Light>();
            if (slotSensorData.Letter == letterCubeData.GetLetterOnCube() && !slotSensorLight.enabled)
            {

                availableHints--;
                slotSensorLight.enabled = true;
                Debug.Log("Look for illuminated slot");
                return;

            }
        }
        // transform.GetChild(slotSensorToHighlight).GetComponent<Light>().enabled = true;

    }
}
