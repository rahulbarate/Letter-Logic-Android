using UnityEngine;

public class LetterCubeMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
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
        float xVal = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        float zVal = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        transform.Translate(xVal, 0f, zVal);
    }
}
