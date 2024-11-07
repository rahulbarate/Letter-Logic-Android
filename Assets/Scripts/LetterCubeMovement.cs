using UnityEngine;

public class LetterCubeMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float lerpSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // disabling cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovement();

        //enabling cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void ProcessMovement()
    {
        // float xVal = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        float xVal = Input.GetAxis("Horizontal");
        // float zVal = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;
        float zVal = Input.GetAxis("Vertical");

        // transform.Translate(xVal, 0f, zVal);

        Vector3 targetPosition = transform.position + movementSpeed * Time.deltaTime * new Vector3(xVal, 0f, zVal);

        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector3 pos)
    {
        // Debug.Log("pos: " + pos);
        transform.localPosition = pos;
    }
}
