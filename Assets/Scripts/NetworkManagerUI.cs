using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(OnHostButtonClicked);
        joinButton.onClick.AddListener(OnJoinButtonClicked);
    }

    private void OnHostButtonClicked()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void OnJoinButtonClicked()
    {
        NetworkManager.Singleton.StartClient();
    }
}
