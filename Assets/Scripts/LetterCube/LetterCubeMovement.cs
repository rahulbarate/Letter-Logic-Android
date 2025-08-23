using System;
using UnityEngine;

public class LetterCubeMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    float movementSpeedAtRunTime;
    [SerializeField] float lerpSpeed = 20f;
    [SerializeField] float jumpForce = 10f;
    bool isGrounded;
    bool isInTheSlot = false;

    LetterCubeData letterCubeData;
    Rigidbody rgbody;

    // Start is called before the first frame update
    void Start()
    {
        // disabling cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        letterCubeData = GetComponent<LetterCubeData>();
        rgbody = GetComponent<Rigidbody>();
        movementSpeedAtRunTime = movementSpeed;


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
        if (isGrounded)
        {
            movementSpeedAtRunTime = movementSpeed;
        }
        else
        {
            movementSpeedAtRunTime = movementSpeed / 3;
        }
        float xVal = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeedAtRunTime;
        float zVal = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeedAtRunTime;
        transform.Translate(new Vector3(xVal, 0f, zVal));
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rgbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Playground"))
        {
            isGrounded = true;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Playground"))
        {
            isGrounded = false;
        }
    }

    public void MoveToInitialPosition()
    {
        if (letterCubeData != null)
        {
            transform.localPosition = letterCubeData.initialPosition;
        }
        else
        {
            Debug.LogError("LetterCubeData is null. Cannot move to initial position.");
        }
    }
}
