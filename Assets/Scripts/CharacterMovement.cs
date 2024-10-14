using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float movementSpeeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        BackForthMovement();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Unlock the cursor
            Cursor.lockState = CursorLockMode.None;

            // Make the cursor visible again
            Cursor.visible = true;
        }
    }
    private void BackForthMovement()
    {
        float xVal = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeeed;
        float zVal = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeeed;
        transform.Translate(xVal, 0f, zVal);
    }
}
