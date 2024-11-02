using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSenserData : MonoBehaviour
{
    [SerializeField] char letter; // field
    public char Letter // property
    {
        get { return letter; }
        set { letter = value; }
    }
}
