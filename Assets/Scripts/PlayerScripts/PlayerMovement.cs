using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement, IControllable
{
    [SerializeField] private InputActionReference _moveReference;

    [SerializeField] private bool _followCamera = true;

    public ActionMap actionMap => ActionMap.Player;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner) return;

        _moveReference.action.performed += OnMove;
        _moveReference.action.canceled += OnMove;

        PlayerInputManager.instance.SubscribeToControllable(this);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if(!IsOwner) return;

        _moveReference.action.performed -= OnMove;
        _moveReference.action.canceled -= OnMove;

        // unsub
    }

    public void InputEnabled()
    {
        
    }

    public void InputDisabled()
    {
        
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _movementInput = new Vector3(input.x, 0f, input.y);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!IsOwner)
            return;

        Vector3 cameraEulerAngles = CameraManager.instance.playerCamera.transform.eulerAngles;

        if (_followCamera)
            _rigidBody.MoveRotation(Quaternion.Euler(0f, cameraEulerAngles.y, 0f));
    }
}
