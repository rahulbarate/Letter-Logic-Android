using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSave", menuName = "Scriptble Objects/GameDataSave")]
public class GameDataSave : ScriptableObject
{
    int totalAvailableHints = 2;
    public int TotalAvailableHints
    {
        get { return totalAvailableHints; }
        set { totalAvailableHints = value; }
    }



    Word lastUsedWord3 = new Word();
    public Word LastUsedWord3
    {
        get { return lastUsedWord3; }
        set { lastUsedWord3 = value; }
    }
    Word lastUsedWord4 = new Word();
    public Word LastUsedWord4
    {
        get { return lastUsedWord4; }
        set { lastUsedWord4 = value; }
    }

    Word lastUsedWord5 = new Word();
    public Word LastUsedWord5
    {
        get { return lastUsedWord5; }
        set { lastUsedWord5 = value; }
    }
}
