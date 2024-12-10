using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSave", menuName = "Scriptble Objects/GameDataSave")]
public class GameDataSave : ScriptableObject
{
    public event Action E_LevelCompleted;
    bool isLevelCompleted;

    // private void OnEnable()
    // {
    //     Application.quitting += RestoreDataToDefault;
    // }

    // private void OnDisable()
    // {
    //     Application.quitting -= RestoreDataToDefault;
    // }
    public bool IsLevelCompleted
    {
        get { return isLevelCompleted; }
        set
        {
            if (value == true && E_LevelCompleted != null)
            {
                E_LevelCompleted.Invoke();
            }
            isLevelCompleted = value;
        }
    }

    PlaygroundType playgroundType;
    public PlaygroundType PlaygroundType
    {
        get { return playgroundType; }
        set { playgroundType = value; }
    }

    AlphabetSubtype alphabetSubtype;
    public AlphabetSubtype AlphabetSubtype
    {
        get { return alphabetSubtype; }
        set { alphabetSubtype = value; }
    }

    NumberSubtype numberSubtype;
    public NumberSubtype NumberSubtype
    {
        get { return numberSubtype; }
        set { numberSubtype = value; }
    }

    WordSubtype wordSubtype;
    public WordSubtype WordSubtype
    {
        get { return wordSubtype; }
        set { wordSubtype = value; }
    }

    MathSubtype mathSubtype;
    public MathSubtype MathSubtype
    {
        get { return mathSubtype; }
        set { mathSubtype = value; }
    }

    GameObject letterCube;
    public GameObject LetterCube
    {
        get { return letterCube; }
        set
        {
            if (value != null && value.GetComponent<LetterCubeData>() != null)
            {
                letterCube = value;
            }
            else
            {
                letterCube = null;
                // Debug.LogWarning("Invalid Object assigned");
            }
        }
    }


    private void RestoreDataToDefault()
    {
        IsLevelCompleted = false;
        PlaygroundType = PlaygroundType.Alphabet;
        AlphabetSubtype = AlphabetSubtype.C_Letters;
        NumberSubtype = NumberSubtype.E_Numbers;
        WordSubtype = WordSubtype.Word_3;
        MathSubtype = MathSubtype.Addition;
        LetterCube = null;
    }



}
