using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkControllable : NetworkBehaviour
{
    private NetworkObject _networkObject;

    [SerializeField]
    private Logger _logger;

    public event Action OnOwnershipGained;
    public event Action OnOwnershipLost;

    public NetworkVariable<ulong> occupier = new NetworkVariable<ulong>(ulong.MaxValue, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public void Awake()
    {
        _networkObject = GetComponent<NetworkObject>();
    }

    public override void OnNetworkSpawn()
    {
        occupier.OnValueChanged += OnOccupierValueChanged;
    }

    public override void OnNetworkDespawn()
    {
        occupier.OnValueChanged -= OnOccupierValueChanged;
    }

    private void OnOccupierValueChanged(ulong last, ulong first)
    {
        if (first == last)
            return;

        if (first == NetworkManager.LocalClientId)
            OnOwnershipGained?.Invoke();
        else if (last == NetworkManager.LocalClientId)
            OnOwnershipLost?.Invoke();
    }

    public bool IsOccupied()
    {
        return (occupier.Value != ulong.MaxValue);
    }

    public bool IsOccupier()
    {
        return (occupier.Value == NetworkManager.LocalClientId);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void RequestOwnershipRpc(RpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        if (IsOccupied())
        {
            _logger.Log($"Client:{clientId} ownership transfer failed, owner is Client:{occupier.Value}.");
            return;
        }

        _logger.Log($"Client:{clientId} ownership transfer succeeded, {transform.gameObject.name} owner has been transfered to Client{_networkObject.OwnerClientId}.");

        occupier.Value = clientId;
        _networkObject.ChangeOwnership(rpcParams.Receive.SenderClientId);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Owner)]
    public void ReleaseOwnershipRPC(RpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        /// We need to sanity check this shit
        if (!IsOccupied())
        {
            _logger.Log($"Client:{clientId} ownership release failed, owner is Client:{occupier.Value}.");
            return;
        }

        _logger.Log($"Client:{clientId} ownership release succeeded, {transform.gameObject.name} owner has been removed.");

        occupier.Value = ulong.MaxValue;
        _networkObject.RemoveOwnership();
    }
}
