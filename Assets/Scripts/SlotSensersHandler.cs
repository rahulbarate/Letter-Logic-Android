using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSensersHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AssignLettersToSlotSensers();
    }

    private void AssignLettersToSlotSensers()
    {
        char ch = 'A';
        foreach (Transform child in transform)
        {
            child.GetComponent<SlotSenserData>().Letter = ch;
            ch++;
        }
    }
}
