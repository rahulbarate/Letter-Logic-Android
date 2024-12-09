using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class LetterCubeData : MonoBehaviour
{
    [SerializeField] string letterOnTop;
    [SerializeField] Transform sideLetters;
    [SerializeField] Transform topFaceLetter;

    // [SerializeField] LetterCubeState letterCubeState = LetterCubeState.Idle;
    public Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
    }




    // public void SetLetterCubeState(LetterCubeState letterCubeState)
    // {
    //     this.letterCubeState = letterCubeState;
    // }
    // public LetterCubeState GetLetterCubeState()
    // {
    //     return this.letterCubeState;
    // }

    public string GetLetterOnCube()
    {
        return letterOnTop;
    }
    public void SetLetterOnCube(string ch, GameObject letterCopy)
    {
        letterOnTop = ch;
        if (sideLetters && sideLetters.transform.childCount > 0)
        {
            foreach (Transform child in sideLetters)
            {
                GameObject instantiatedLetter = Instantiate(letterCopy, child.transform);
                instantiatedLetter.transform.localPosition = Vector3.zero;
                instantiatedLetter.transform.eulerAngles = child.transform.eulerAngles;
                instantiatedLetter.transform.localScale = Vector3.one;
            }
        }
        if (topFaceLetter)
        {
            GameObject instantiatedLetter = Instantiate(letterCopy, topFaceLetter.transform);
            instantiatedLetter.transform.localPosition = Vector3.zero;
            instantiatedLetter.transform.eulerAngles = topFaceLetter.transform.eulerAngles;
            instantiatedLetter.transform.localScale = Vector3.one;
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Playground"))
    //     {
    //         initialPosition = transform.localPosition;
    //     }
    // }

}
