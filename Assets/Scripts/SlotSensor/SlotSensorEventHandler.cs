using System;
using UnityEngine;

public class SlotSensorEventHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Letter Cube"))
        {
            // Debug.Log(other.GetComponent<LetterCubeData>().GetLetterOnCube());
            // Debug.Log(other.GetComponent<LetterCubeData>().GetLetterOnCube()[0]);
            char ch = other.GetComponent<LetterCubeData>().GetLetterOnCube()[0];
            int index = Convert.ToInt32(GetComponent<SlotSensorData>().Letter);
            GetComponentInParent<SlotSensorsHandler>().SetCharInUserCreatedWord(index, ch);
        }
    }
}