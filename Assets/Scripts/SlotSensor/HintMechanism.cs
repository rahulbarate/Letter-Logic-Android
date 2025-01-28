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
        GameObject letterCube = gameDataSave.LetterCube;
        if (availableHints <= 0)
        {
            Debug.Log("No hints available!");
            return;
        }

        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf == true)
            {
                availableHints--;
                child.GetComponent<Light>().enabled = true;
                return;
            }
        }

    }
}
