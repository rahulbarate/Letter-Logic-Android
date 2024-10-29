using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class LetterCubeData : MonoBehaviour
{
    [SerializeField] char letterOnTop;
    [SerializeField] Transform sideLetters;
    [SerializeField] Transform sideLawnLayers;
    [SerializeField] Transform topFaceLetter;

    [SerializeField] LetterCubeState letterCubeState = LetterCubeState.Idle;

    Rigidbody rgbody;
    LetterCubeHandler letterCubeHandler;
    LetterCubeMovement letterCubeMovement;

    // Start is called before the first frame update
    void Start()
    {
        rgbody = GetComponent<Rigidbody>();
        letterCubeHandler = GetComponent<LetterCubeHandler>();
        letterCubeMovement = GetComponent<LetterCubeMovement>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ProcessCorrectLetterCube()
    {
        Destroy(rgbody);
        Destroy(letterCubeHandler);
        Destroy(sideLetters.gameObject);
        Destroy(sideLawnLayers.gameObject);
        Destroy(letterCubeMovement);
    }



    public void SetLetterCubeState(LetterCubeState letterCubeState)
    {
        this.letterCubeState = letterCubeState;
    }
    public LetterCubeState GetLetterCubeState()
    {
        return this.letterCubeState;
    }

    public char GetLetterOnCube()
    {
        return letterOnTop;
    }
    public void SetLetterOnCube(char ch, GameObject letterCopy)
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

}
