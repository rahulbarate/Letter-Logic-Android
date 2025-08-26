using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public static InputReader Instance { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    [SerializeField] private InputActionAsset inputAsset;
    private InputAction moveAction;
    private InputAction lookAction;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        var map = inputAsset.FindActionMap("LetterCube");
        moveAction = map.FindAction("Move");
        lookAction = map.FindAction("Look");

        moveAction.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += _ => MoveInput = Vector2.zero;

        lookAction.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        lookAction.canceled += _ => LookInput = Vector2.zero;

        moveAction.Enable();
        lookAction.Enable();
    }
}
