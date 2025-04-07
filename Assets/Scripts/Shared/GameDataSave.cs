using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSave", menuName = "Scriptble Objects/GameDataSave")]
public class GameDataSave : ScriptableObject
{
    public event Action E_LevelCompleted;

    // public event Action E_WordCompleted;
    // public event Action E_PlacedInSlot;

    Word currentWord;
    public Word CurrentWord
    {
        get { return currentWord; }
        set { currentWord = value; }
    }

    private List<char> userCreatedWord;
    int noOfElementsInUserCreatedWord;
    public void SetCharInUserCreatedWord(int index, char ch)
    {
        userCreatedWord[index] = ch;
        noOfElementsInUserCreatedWord++;
        if (noOfElementsInUserCreatedWord == WordLength)
        {
            IsWordCompleted = true;
            // E_WordCompleted?.Invoke();
        }
        // else{
        //     E_PlacedInSlot?.Invoke();
        // }
    }
    public void InitializeUserCreatedWord()
    {
        userCreatedWord = new List<char>(new char[WordLength]);
        noOfElementsInUserCreatedWord = 0;
    }

    public List<char> GetUserCreatedWord()
    {
        return userCreatedWord;
    }

    Word firstWord3 = new Word();
    public Word FirstWord3
    {
        get { return firstWord3; }
        set { firstWord3 = value; }
    }
    Word firstWord4 = new Word();
    public Word FirstWord4
    {
        get { return firstWord4; }
        set { firstWord4 = value; }
    }

    Word firstWord5 = new Word();
    public Word FirstWord5
    {
        get { return firstWord5; }
        set { firstWord5 = value; }
    }




    bool isWordCompleted = false;
    public bool IsWordCompleted
    {
        get { return isWordCompleted; }
        set { isWordCompleted = value; }
    }


    int wordLength = 3;
    public int WordLength
    {
        get { return wordLength; }
        set { wordLength = value; }
    }
    bool isLevelCompleted;
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
