using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCubeInstantiator : MonoBehaviour
{
    public static GameObject InstantiateLetterCube(GameObject letterCubeCopy, Vector3 spawnPosition, float uniformScale, String letter, GameObject letterOnSides, bool isLetterCubeMoveable)
    {
        GameObject letterCube;

        letterCube = Instantiate(letterCubeCopy, spawnPosition, Quaternion.identity);
        letterCube.transform.localScale = new Vector3(uniformScale, uniformScale, uniformScale);
        if (isLetterCubeMoveable)
        {
            letterCube.GetComponent<LetterCubeMovement>().enabled = true;
        }
        else
        {
            letterCube.GetComponent<LetterCubeMovement>().enabled = false;
        }
        if (letterCube.GetComponent<LetterCubeData>() == null)
        {
            letterCube.AddComponent<LetterCubeData>();
        }
        if (letterCube.GetComponent<LetterCubeEventHandler>() == null)
        {
            letterCube.AddComponent<LetterCubeEventHandler>();
        }
        letterCube.GetComponent<LetterCubeData>().SetLetterOnCube(letter, letterOnSides);


        return letterCube;
    }

}
