using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintMechanism : MonoBehaviour
{
    [SerializeField] public int availableHints = 30;
    [SerializeField] GameObject slotSensorsParent;
    [SerializeField] TextMeshProUGUI hintDisplayText;
    [SerializeField] Spawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        hintDisplayText.text = availableHints.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ProcessHint();
        }
    }

    public void ProcessHint()
    {
        if (availableHints <= 0)
        {
            Debug.Log("No hints available!");
            return;
        }

        foreach (int index in spawner.correctSlotSensorIndex)
        {
            // CustomLogger.Log(index);
            GameObject slotSensor = slotSensorsParent.transform.GetChild(index).gameObject;
            if (slotSensor != null)
            {
                if (!slotSensor.GetComponent<Light>().enabled)
                {
                    --availableHints;
                    slotSensor.GetComponent<Light>().enabled = true;
                    hintDisplayText.text = availableHints.ToString();
                }

            }
        }
        spawner.correctSlotSensorIndex.Clear();
    }
}
