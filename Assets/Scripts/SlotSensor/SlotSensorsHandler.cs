using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSensorsHandler : MonoBehaviour
{
    [SerializeField] GameDataSave gameDataSave;
    // Start is called before the first frame update
    void Start()
    {
        // AssignCLettersToSlotSensers();
    }

    public void SetCharInUserCreatedWord(int index, char ch)
    {
        gameDataSave.SetCharInUserCreatedWord(index, ch);
    }

    public void AssignCLettersToSlotSensors()
    {
        char ch = 'A';
        foreach (Transform child in transform)
        {
            child.GetComponent<SlotSensorData>().Letter = ch.ToString();
            ch++;
        }
    }
    public void AssignENumbersToSlotSensors()
    {
        int i = 1;
        foreach (Transform child in transform)
        {
            child.GetComponent<SlotSensorData>().Letter = i.ToString();
            i++;
        }
    }
    public void SetActive(int index)
    {
        // Debug.Log(index + " to Set Active");
        transform.GetChild(index).gameObject.SetActive(true);
    }

    public void SetAllActive()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
