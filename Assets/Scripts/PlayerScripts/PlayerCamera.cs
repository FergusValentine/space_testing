using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : NetworkBehaviour, IControllable
{
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Camera _skyboxCamera;
    [SerializeField] private float _mouseSensitivity = 100f;

    [SerializeField] private InputActionReference _lookReference;
    
    private Vector2 _lookInput;
    private float _xRotation;
    private float _yRotation;
    
    public ActionMap actionMap => ActionMap.Player;

    private bool IsEnabled;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        _playerCamera.enabled = true;
        _skyboxCamera.enabled = true;

        _lookReference.action.performed += OnLook;
        _lookReference.action.canceled += OnLook;

        PlayerInputManager.instance.SubscribeToControllable(this);
        CameraManager.instance.SetPlayerCamera(_playerCamera);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (!IsOwner) return;

        _lookReference.action.performed -= OnLook;
        _lookReference.action.canceled -= OnLook;
    }

    public void InputEnabled()
    {
        IsEnabled = true;

        _yRotation = 0.0f;
        _xRotation = 0.0f;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void InputDisabled()
    {
        IsEnabled = false;

        Cursor.lockState = CursorLockMode.None;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if(!IsOwner) return;

        _yRotation += _lookInput.x * _mouseSensitivity * Time.deltaTime;
        _xRotation -= _lookInput.y * _mouseSensitivity * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        if (IsEnabled)
            _playerCamera.transform.SetPositionAndRotation(_cameraPosition.position, Quaternion.Euler(_xRotation, _yRotation, 0f));

        _skyboxCamera.transform.localRotation = Quaternion.Inverse(SkyboxManager.instance.GetSkyboxRotation()) * _playerCamera.transform.rotation;
    }
}
