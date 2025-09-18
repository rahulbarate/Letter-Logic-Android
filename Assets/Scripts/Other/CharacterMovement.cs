using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float movementSpeeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        BackForthMovement();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Unlock the cursor
            UnityEngine.Cursor.lockState = CursorLockMode.None;

            // Make the cursor visible again
            UnityEngine.Cursor.visible = true;
        }
    }
    private void BackForthMovement()
    {
        float xVal = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeeed;
        float zVal = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeeed;
        transform.Translate(xVal, 0f, zVal);
        // transform.Translate(0f, 0f, zVal);
        // if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     if (!isRotating)
        //     {
        //         targetRotation -= 90f;
        //         isRotating = true;
        //     }
        // }
        // else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     if (!isRotating)
        //     {
        //         targetRotation += 90f;
        //         isRotating = true;
        //     }
        // }

        // if (isRotating)
        // {
        //     float currentRotation = transform.rotation.eulerAngles.y;
        //     float nextRotation = Mathf.MoveTowardsAngle(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        //     transform.rotation = Quaternion.Euler(0f, nextRotation, 0f);
        //     if (Mathf.Approximately(nextRotation, targetRotation))
        //     {
        //         isRotating = false;
        //     }
        // }

    }
}
