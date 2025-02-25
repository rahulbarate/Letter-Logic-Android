using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSensorData : MonoBehaviour
{
    [SerializeField] string letter = null; // field
    public string Letter // property
    {
        get { return letter; }
        set { letter = value; }
    }
}
