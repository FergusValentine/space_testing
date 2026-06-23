using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceCoop.ShipSystems.ShipControl
{
    public class ShipHelmPlayer : NetworkBehaviour, IControllable, IInteractable
    {
        public ActionMap actionMap { get { return ActionMap.ShipHelm; } }

        [SerializeField] private InputActionReference _moveReference;
        [SerializeField] private InputActionReference _lookReference;
        [SerializeField] private InputActionReference _rollReference;
        [SerializeField] private InputActionReference _exitReference;

        private NetworkControllable _controllable;
        private ClientInput _helmModel;
        private ClientView _shipView;
        private HelmNetwork _shipServer;

        private void Awake()
        {
            _controllable = GetComponent<NetworkControllable>();
            _shipServer = GetComponent<HelmNetwork>();
            _helmModel = new ClientInput();
            _shipView = new ClientView();
        }

        public override void OnNetworkSpawn()
        {
            _controllable.OnOwnershipGained += EnableControllable;
            _controllable.OnOwnershipLost += DisableControllable;
        }

        public override void OnNetworkDespawn()
        {
            _controllable.OnOwnershipGained -= EnableControllable;
            _controllable.OnOwnershipLost -= DisableControllable;

            if (_controllable.IsOccupier())
                DisableControllable();
        }

        private void EnableControllable()
        {
            PlayerInputManager.instance.SubscribeToControllable(this);
        }

        private void DisableControllable()
        {
            PlayerInputManager.instance.UnsubscribeControllable(this);
        }

        public void InputEnabled()
        {
            _moveReference.action.performed += OnMove;
            _moveReference.action.canceled += OnMove;
            _lookReference.action.performed += OnLook;
            _lookReference.action.canceled += OnLook;
            _rollReference.action.performed += OnRoll;
            _rollReference.action.canceled += OnRoll;
            _exitReference.action.started += OnExit;

            _shipView.OnEnable(transform);
        }

        public void InputDisabled()
        {
            _moveReference.action.performed -= OnMove;
            _moveReference.action.canceled -= OnMove;
            _lookReference.action.performed -= OnLook;
            _lookReference.action.canceled -= OnLook;
            _rollReference.action.performed -= OnRoll;
            _rollReference.action.canceled -= OnRoll;
            _exitReference.action.started -= OnExit;

            _shipView.OnDisable();
        }

        public bool IsInteractable()
        {
            return (!_controllable.IsOccupied());
        }

        public void Interact()
        {
            if (!IsInteractable())
                return;

            _controllable.RequestOwnershipRpc();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (!_controllable.IsOccupier()) return;
           
            _helmModel.movementInput = context.ReadValue<Vector3>();
            _shipServer._movementInput.Value = _helmModel.movementInput;
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            if (!_controllable.IsOccupier()) return;

            Vector2 lookInput = context.ReadValue<Vector2>();

            float deadZone = 0.1f;

            Vector2 screenCentre = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 mouseOffset = (lookInput - screenCentre) / screenCentre;

            if (mouseOffset.magnitude > deadZone)
                _helmModel.steeringInput = mouseOffset.normalized * ((mouseOffset.magnitude - deadZone) / (1f - deadZone));
            else
                _helmModel.steeringInput = Vector2.zero;
            _shipServer._mousePosition.Value = _helmModel.steeringInput;
        }

        private void OnRoll(InputAction.CallbackContext context)
        {
            if (!_controllable.IsOccupier()) return;

            _helmModel.rollInput = context.ReadValue<float>();
            _shipServer._rollInput.Value = _helmModel.rollInput;
        }

        private void OnExit(InputAction.CallbackContext context)
        {
            if (!_controllable.IsOccupier()) return;

            _controllable.ReleaseOwnershipRPC();
        }
    }
}
