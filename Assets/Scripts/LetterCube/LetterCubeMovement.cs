using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class LetterCubeMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] private float verticalLookSensitivity = 1f;
    [SerializeField] private float horizontalLookSensitivity = 20f;
    float movementSpeedAtRunTime;

    [SerializeField] Cinemachine.CinemachineFreeLook freeLookCamera;

    public GameObject activeLetterCube = null;

    // public LetterCubeData letterCubeData;
    Rigidbody rgbody;

    // Start is called before the first frame update
    void Start()
    {
        // disabling cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // letterCubeData = activeLetterCube.GetComponent<LetterCubeData>();
        rgbody = GetComponent<Rigidbody>();
        movementSpeedAtRunTime = movementSpeed;

        // check for cinemachine cam
        freeLookCamera = FindObjectOfType<Cinemachine.CinemachineFreeLook>();
        if (freeLookCamera == null)
        {
            Debug.LogError("CinemachineFreeLook camera not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activeLetterCube == null)
            return;
        Vector2 moveInput = InputReader.Instance.MoveInput;
        Vector2 lookInput = InputReader.Instance.LookInput;

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * movementSpeed * Time.deltaTime;
        activeLetterCube.transform.Translate(move);

        freeLookCamera.m_XAxis.Value += lookInput.x * horizontalLookSensitivity * Time.deltaTime;
        freeLookCamera.m_YAxis.Value -= lookInput.y * verticalLookSensitivity * Time.deltaTime;
    }

    public void MoveToInitialPosition()
    {
        if (activeLetterCube != null && activeLetterCube.TryGetComponent(out LetterCubeData letterCubeData))
        {
            activeLetterCube.transform.localPosition = letterCubeData.initialPosition;
        }
        else
        {
            Debug.LogError("LetterCubeData is null. Cannot move to initial position.");
        }
    }
}
