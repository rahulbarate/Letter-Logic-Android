
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

    public ParticleSystem swirlEffect;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(transform.localPosition);
        initialPosition = transform.localPosition;

        // Debug.Log($"{initialPosition.x}-{initialPosition.y}-{initialPosition.z}");
    }
    void OnEnable()
    {
        swirlEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
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
