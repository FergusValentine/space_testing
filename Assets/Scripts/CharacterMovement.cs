using Unity.Netcode;
using UnityEngine;

public class CharacterMovement : NetworkBehaviour
{
    protected Rigidbody _rigidBody;

    [SerializeField] private float _movementSpeed = 5f;

    protected Vector3 _movementInput;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }
    
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }

    protected virtual void FixedUpdate()
    {
        if (!IsOwner)
            return;

        Vector3 movementDirection = transform.TransformDirection(_movementInput).normalized * _movementSpeed;
        _rigidBody.MovePosition(_rigidBody.position + movementDirection * Time.fixedDeltaTime);
    }
}
