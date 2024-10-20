
using UnityEngine;
using Unity.Netcode;

public class PlayersReady : NetworkBehaviour
{
    [SerializeField] private WaitingPlayersUi toggleReady;
    
    private void Awake()
    {
        toggleReady.OnPlayerToggleReady += ToggleReady_OnPlayerReady;
    }
    
    private void ToggleReady_OnPlayerReady(bool obj)
    {
        KitchenGameMultiPlayer.Instance.SetPlayerAsReadyServerRpc(obj);
    }
    
   
}
