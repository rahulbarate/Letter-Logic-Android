using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSensorsHandler : MonoBehaviour
{
    [SerializeField] GameDataSave gameDataSave;

    List<SlotSensorHandler> childrenSlotSensorData;
    // Start is called before the first frame update
    void Start()
    {
        childrenSlotSensorData = new();
        // get slot sensor data of every child
        foreach (Transform child in transform)
        {
            childrenSlotSensorData.Add(child.GetComponent<SlotSensorHandler>());
        }
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
            child.GetComponent<SlotSensorHandler>().Letter = ch.ToString();
            ch++;
        }
    }
    public void AssignWord(List<char> wordChoosen)
    {
        // char ch = 'A';
        int i = 0;
        foreach (Transform child in transform)
        {
            child.GetComponent<SlotSensorHandler>().Letter = wordChoosen[i].ToString();
            i++;
        }
    }
    public void AssignENumbersToSlotSensors()
    {
        int i = 1;
        foreach (Transform child in transform)
        {
            child.GetComponent<SlotSensorHandler>().Letter = i.ToString();
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
