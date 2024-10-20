
using UnityEngine;
using UnityEngine.UI;

public class TestingLobbyUi : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button joinGameButton;

    private void Awake()
    {
        startGameButton.onClick.AddListener(() =>
        {
            KitchenGameMultiPlayer.Instance.StartHost();
            Loader.Network_LoadSceneMode(Loader.Scene.CharacterSelectScene);
        });
        joinGameButton.onClick.AddListener(() =>
        {
            KitchenGameMultiPlayer.Instance.StartClient();
        });
    }
}
