using System;
using Unity.Netcode;
using UnityEngine;

namespace SpaceCoop.ShipSystems.ShipControl
{
    public class HelmNetwork : NetworkBehaviour
    {
        [HideInInspector] public NetworkVariable<Vector3> _movementInput = new NetworkVariable<Vector3>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
        [HideInInspector] public NetworkVariable<Vector2> _mousePosition = new NetworkVariable<Vector2>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
        [HideInInspector] public NetworkVariable<float> _rollInput = new NetworkVariable<float>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);

        private ClientInput _model;
        private ShipView _view;

        private void Awake()
        {
            _model = new ClientInput();
            _view = GetComponent<ShipView>();
        }

        public override void OnNetworkSpawn()
        {
            _movementInput.OnValueChanged += OnMovementInput;
            _mousePosition.OnValueChanged += OnLookInput;
            _rollInput.OnValueChanged += OnRollInput;
        }

        public override void OnNetworkDespawn()
        {
            _movementInput.OnValueChanged -= OnMovementInput;
            _mousePosition.OnValueChanged -= OnLookInput;
            _rollInput.OnValueChanged -= OnRollInput;
        }

        private void OnMovementInput(Vector3 oldValue, Vector3 newValue)
        {
            _model.movementInput = newValue;
        }

        private void OnLookInput(Vector2 oldValue, Vector2 newValue)
        {
            _model.steeringInput = newValue;
        }

        private void OnRollInput(float oldValue, float newValue)
        {
            _model.rollInput = newValue;
        }

        private void Update()
        {
            if(!IsOwner) return;
            
            _view.UpdateSteering(_model.movementInput, _model.steeringInput);
        }
    }
}