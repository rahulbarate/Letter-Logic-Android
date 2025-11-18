using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class LetterCubeMovement : MonoBehaviour
{
    [SerializeField] float defaultMovementSpeed = 5f;
    float movementSpeed;
    [SerializeField] float powerUpMovementSpeed = 10f;
    [SerializeField] private float verticalLookSensitivity = 1f;
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private float horizontalLookSensitivity = 20f;
    float movementSpeedAtRunTime;

    [SerializeField] Cinemachine.CinemachineFreeLook freeLookCamera;

    GameObject activeLetterCube = null;
    public GameObject ActiveLetterCube
    {
        get { return activeLetterCube; }
        set
        {
            activeLetterCube = value;
            if (activeLetterCube != null) rgbody = activeLetterCube.GetComponent<Rigidbody>();
        }
    }
    Vector2 moveInput;
    Vector2 lookInput;
    InputAction moveAction;
    InputAction lookAction;

    // public LetterCubeData letterCubeData;
    Rigidbody rgbody;

    void Awake()
    {

        var map = inputAsset.FindActionMap("LetterCube");
        moveAction = map.FindAction("Move");
        lookAction = map.FindAction("Look");

        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += _ => moveInput = Vector2.zero;

        lookAction.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        lookAction.canceled += _ => lookInput = Vector2.zero;

        moveAction.Enable();
        lookAction.Enable();
    }



    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = defaultMovementSpeed;
        // disabling cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // letterCubeData = activeLetterCube.GetComponent<LetterCubeData>();
        movementSpeedAtRunTime = movementSpeed;

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
        // Vector2 moveInput = InputReader.Instance.MoveInput;
        // Vector2 lookInput = InputReader.Instance.LookInput;

        freeLookCamera.m_XAxis.Value += lookInput.x * horizontalLookSensitivity * Time.deltaTime;
        freeLookCamera.m_YAxis.Value -= lookInput.y * verticalLookSensitivity * Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (activeLetterCube == null || rgbody == null)
            return;

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * movementSpeed * Time.fixedDeltaTime;
        rgbody.MovePosition(rgbody.position + move);
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

    public void ToggleMovementSpeed()
    {
        if (movementSpeed == defaultMovementSpeed)
            movementSpeed = powerUpMovementSpeed;
        else
            movementSpeed = defaultMovementSpeed;
    }
}
