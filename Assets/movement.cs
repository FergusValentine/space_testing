using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class movement : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    private Vector3 m_move_input;
  
    private Vector2 m_look_input;
    private Vector2 m_mouse_input;

    private Vector3 m_linear_velocity;
    private Vector3 m_rotation_velocity;

    public float max_rotation_speed = 1f;
    public float rotational_acceleration = 1f;
    public float rotational_deceleration = 1f;

    public float max_linear_speed = 1f;
    public float linear_acceleration = 1f;
    public float linear_deceleration = 1f;

    [SerializeField] private Transform ship;
    [SerializeField] private Transform environment;

    [SerializeField] private Transform main_camera;
    [SerializeField] private Transform skybox;
    [SerializeField] private Transform skybox_direction;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Look.canceled += OnLook;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();

        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Look.canceled -= OnLook;

    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        m_move_input.x = input.x;
        m_move_input.y = input.y;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        m_look_input = context.ReadValue<Vector2>();
    }

    private void OnMouse(InputAction.CallbackContext context)
    {
        m_mouse_input = context.ReadValue<Vector2>();
    }

    private void PivotQ(InputAction.CallbackContext context)
    {
        m_move_input.z = -context.ReadValue<float>();
    }

    private void PivotE(InputAction.CallbackContext context)
    {
        m_move_input.z = context.ReadValue<float>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(-m_move_input.x, 0f, -m_move_input.y).normalized;
        movement = ship.TransformDirection(movement);

        if (movement.magnitude > 0.1f)
        {
            m_linear_velocity += movement * linear_acceleration * Time.deltaTime;
        }
        else
        {
            m_linear_velocity = Vector3.MoveTowards(m_linear_velocity, Vector3.zero, linear_deceleration * Time.deltaTime);
        }
        m_linear_velocity = Vector3.ClampMagnitude(m_linear_velocity, max_linear_speed);

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 mouseOffset = (m_mouse_input - screenCenter) / screenCenter;
        mouseOffset = Vector3.ClampMagnitude(mouseOffset, 1f);

        float deadzone = 0.025f;
        mouseOffset = mouseOffset.normalized * ((mouseOffset.magnitude - deadzone) / (1f - deadzone));

        if (mouseOffset.sqrMagnitude < deadzone)
        {
            mouseOffset = Vector2.zero;
        }

        Vector3 steering = new Vector3(mouseOffset.y, -mouseOffset.x, m_move_input.z);
        m_rotation_velocity += steering * rotational_acceleration * Time.deltaTime;

        if (steering.magnitude < 0.1f)
        {
            m_rotation_velocity -= m_rotation_velocity.normalized * rotational_deceleration * Time.deltaTime;
        }
        m_rotation_velocity = Vector3.ClampMagnitude(m_rotation_velocity, max_rotation_speed);


        Matrix4x4 offset = Matrix4x4.TRS(m_linear_velocity, Quaternion.Euler(m_rotation_velocity * Time.deltaTime), Vector3.one);
        Matrix4x4 shipSpace = offset * ship.localToWorldMatrix;

        foreach (Transform child in environment)
        {
            Matrix4x4 environmentSpace = ship.worldToLocalMatrix * child.localToWorldMatrix;
            Matrix4x4 newEnvironmentSpace = shipSpace * environmentSpace;

            child.position = newEnvironmentSpace.GetPosition();
            child.rotation = Quaternion.Normalize(newEnvironmentSpace.rotation);
        }

        Vector2 mouseInput = m_mouse_input * 5 * Time.deltaTime;

        transform.localRotation *= Quaternion.Euler(0f, m_look_input.x * 5 * Time.fixedDeltaTime, 0f);
        //skybox.localRotation = Quaternion.Inverse(skybox_direction.rotation) * main_camera.transform.rotation;

        //m_rigidbody.linearVelocity = movement;
        //m_rigidbody.MoveRotation(Quaternion.Euler(0f, transform.rotation.eulerAngles.y + m_look_input.x, 0f));
    }
}
