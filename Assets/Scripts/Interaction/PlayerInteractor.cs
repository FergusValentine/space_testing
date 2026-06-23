using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : NetworkBehaviour, IControllable
{
    [SerializeField] private InputActionReference _interactReference;
    [SerializeField] private float _interactionRange = 4f;

    private IInteractable _currentInteractable;
    public ActionMap actionMap => ActionMap.Player;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        _interactReference.action.started += OnInteract;
        PlayerInputManager.instance.SubscribeToControllable(this);
    }

    public override void OnNetworkDespawn()
    {
        if(!IsOwner) return;

        _interactReference.action.started -= OnInteract;
        PlayerInputManager.instance.UnsubscribeControllable(this);
    }

    public void InputEnabled(){}

    public void InputDisabled(){}

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (_currentInteractable == null)
            return;

        if (!_currentInteractable.IsInteractable())
            return;
        
        _currentInteractable.Interact();
    }

    private void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _interactionRange))
        {
            _currentInteractable = hit.collider.GetComponentInParent<IInteractable>(true);
            
            if (_currentInteractable != null && hit.collider.TryGetComponent(out InteractionPrompt prompt))
            {
                if(_currentInteractable.IsInteractable())
                    UIManager.instance.SetPromptUI(prompt);
            }

            return;
        }

        UIManager.instance.SetPromptUIVisible(false);
        _currentInteractable = null;
    }

    private void Update()
    {
        TryInteract();
    }
}