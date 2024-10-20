using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private TMP_InputField codeInputFiled;
    [SerializeField] private CreateLobbyUi createLobbyUi;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.LeaveLobby();
            Loader.LoadSceneMode(Loader.Scene.MainMenu);
        });
        createLobbyButton.onClick.AddListener(() =>
        {
            createLobbyUi.Show();
            
        });
        joinLobbyButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.JoinedLobbyByCode(codeInputFiled.text);
        });
    }
}
