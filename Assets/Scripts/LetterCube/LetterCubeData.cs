
using UnityEngine;

public class LetterCubeData : MonoBehaviour
{
    string letterOnTop;
    public string LetterOnTop
    {
        get { return letterOnTop; }
        set { letterOnTop = value; }
    }
    public Vector3 initialPosition;

    public bool isPlaced = false;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(transform.localPosition);
        initialPosition = transform.localPosition;
    }


    public string GetLetterOnCube()
    {
        return letterOnTop;
    }
    public void SetLetterOnCube(string ch)
    {
        letterOnTop = ch;
    }

}
