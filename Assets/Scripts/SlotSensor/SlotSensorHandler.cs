using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSensorHandler : MonoBehaviour
{
    public string letter = null; // field
    public string Letter // property
    {
        get { return letter; }
        set { letter = value; }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Letter Cube"))
        {
            // transform.GetComponent<Light>().enabled = false;
            transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            // // Debug.Log(other.GetComponent<LetterCubeData>().GetLetterOnCube());
            // // Debug.Log(other.GetComponent<LetterCubeData>().GetLetterOnCube()[0]);
            // char ch = other.GetComponent<LetterCubeData>().LetterOnTop[0];
            // int index = Convert.ToInt32(GetComponent<SlotSensorData>().Letter);
            // GetComponentInParent<SlotSensorsHandler>().SetCharInUserCreatedWord(index, ch);
        }
    }
}
