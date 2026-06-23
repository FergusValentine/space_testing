using Unity.Netcode;
using UnityEngine;

public class ClientControllable : NetworkBehaviour
{
    private NetworkControllable _networkControllable;

    public override void OnNetworkSpawn()
    {
        _networkControllable.occupier.OnValueChanged += OwnershipChanged;
    }

    public override void OnNetworkDespawn()
    {
        _networkControllable.occupier.OnValueChanged -= OwnershipChanged;
    }

    public void OwnershipChanged(ulong previousValue, ulong newValue)
    {
        if (previousValue == newValue)
            return;

        if (newValue == NetworkManager.LocalClientId)
        {
            Debug.Log("Subscribe");
            
        }
        else if (previousValue == NetworkManager.LocalClientId)
        {
            Debug.Log("Unsub");
            
        }
    }
}
