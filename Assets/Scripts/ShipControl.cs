//using NUnit.Framework;
//using System.Collections.Generic;
//using Unity.Netcode;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class ShipControl : NetworkBehaviour
//{
//    // Client //
//    private IInteractor _interactor;
//    public ActionMap actionMap { get { return ActionMap.ShipHelm; } }
    
//    [SerializeField] private InputActionReference _moveReference;
//    [SerializeField] private InputActionReference _lookReference;
//    [SerializeField] private InputActionReference _rollReference;
//    [SerializeField] private InputActionReference _exitReference;

//    // Server //
//    [SerializeField] private Transform _environment;

//    private NetworkVariable<Vector3> _movementInput = new NetworkVariable<Vector3>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
//    private NetworkVariable<Vector2> _mousePosition = new NetworkVariable<Vector2>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
//    private NetworkVariable<float> _rollInput = new NetworkVariable<float>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);

//    private float AccelerateVector(float movement, float velocity, float acceleration, float deceleration, float max)
//    {
//        if (Mathf.Abs(movement) > 0.05f)
//            velocity += movement * acceleration * Time.deltaTime;
//        else
//            velocity = Mathf.MoveTowards(velocity, 0.0f, deceleration * Time.deltaTime);
//        return Mathf.Clamp(velocity, -max, max);
//    }Does t

//    private void Update()
//    {
//        if (!IsServer)
//            return;

//        Vector3 movementInput = _movementInput.Value;
//        Vector3 controlInput = new Vector3(-movementInput.x, -movementInput.y, -movementInput.z).normalized;
//        Vector3 movementDirection = transform.TransformDirection(controlInput);

//        _linearVelocity.x = AccelerateVector(movementDirection.x, _linearVelocity.x, _linearAcceleration.x, _linearDeceleration.x, _maxLinearVelocity.x);
//        _linearVelocity.y = AccelerateVector(movementDirection.y, _linearVelocity.y, _linearAcceleration.y, _linearDeceleration.y, _maxLinearVelocity.y);
//        _linearVelocity.z = AccelerateVector(movementDirection.z, _linearVelocity.z, _linearAcceleration.z, _linearDeceleration.z, _maxLinearVelocity.z);

//        float rollInput = _rollInput.Value;
//        Vector2 mousePosition = _mousePosition.Value;
//        Vector3 steeringInput = new Vector3(mousePosition.y, -mousePosition.x, rollInput);

//        _rotationalVelocity.x = AccelerateVector(steeringInput.x, _rotationalVelocity.x, _rotationalAcceleration.x, _rotationalDecelleration.x, _maxRotationalVelocity.x);
//        _rotationalVelocity.y = AccelerateVector(steeringInput.y, _rotationalVelocity.y, _rotationalAcceleration.y, _rotationalDecelleration.y, _maxRotationalVelocity.y);
//        _rotationalVelocity.z = AccelerateVector(steeringInput.z, _rotationalVelocity.z, _rotationalAcceleration.z, _rotationalDecelleration.z, _maxRotationalVelocity.z);

//        Matrix4x4 offset = Matrix4x4.TRS(_linearVelocity, Quaternion.Euler(_rotationalVelocity * Time.deltaTime), Vector3.one);
//        Matrix4x4 shipSpace = offset * transform.localToWorldMatrix;

//        foreach (Transform child in _environment)
//        {
//            Matrix4x4 environmentSpace = transform.worldToLocalMatrix * child.localToWorldMatrix;
//            Matrix4x4 newEnvironmentSpace = shipSpace * environmentSpace;

//            child.position = newEnvironmentSpace.GetPosition();
//            child.rotation = Quaternion.Normalize(newEnvironmentSpace.rotation);
//        }
//    }
//}
