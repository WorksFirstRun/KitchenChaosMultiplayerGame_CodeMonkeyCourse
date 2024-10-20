using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterUi : MonoBehaviour
{
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCode;
    private void Awake()
    {
        MainMenuButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
            Loader.LoadSceneMode(Loader.Scene.MainMenu);
        });
    }

    private void Start()
    {
        Lobby joinedLobby = KitchenGameLobby.Instance.GetJoinedLobby();
        lobbyNameText.text = "Lobby Name : " + joinedLobby.Name;
        lobbyCode.text = "Lobby Code : " + joinedLobby.LobbyCode;
    }
}
