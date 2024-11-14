using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSave : MonoBehaviour
{
    static bool isLevelCompleted;

    public static bool IsLevelCompleted
    {
        get { return isLevelCompleted; }
        set { isLevelCompleted = value; }
    }

    static PlaygroundType playgroundType;
    public static PlaygroundType PlaygroundType
    {
        get { return playgroundType; }
        set { playgroundType = value; }
    }

    static AlphabetSubtype alphabetSubtype;
    public static AlphabetSubtype AlphabetSubtype
    {
        get { return alphabetSubtype; }
        set { alphabetSubtype = value; }
    }

    static NumberSubtype numberSubtype;
    public static NumberSubtype NumberSubtype
    {
        get { return numberSubtype; }
        set { numberSubtype = value; }
    }

    static WordSubtype wordSubtype;
    public static WordSubtype WordSubtype
    {
        get { return wordSubtype; }
        set { wordSubtype = value; }
    }

    static MathSubtype mathSubtype;
    public static MathSubtype MathSubtype
    {
        get { return mathSubtype; }
        set { mathSubtype = value; }
    }

    static LetterCubeData letterCubeData;
    public static LetterCubeData LetterCubeData
    {
        get { return letterCubeData; }
        set { letterCubeData = value; }
    }



}
