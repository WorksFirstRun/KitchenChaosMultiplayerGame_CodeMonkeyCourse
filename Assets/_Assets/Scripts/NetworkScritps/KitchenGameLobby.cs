using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class KitchenGameLobby : MonoBehaviour
{
   private const string RELAY_JOIN_CODE = "RelayJoinCode";
   public static KitchenGameLobby Instance { get; private set; }
   private Lobby joinedLobby;
   private float heartBeatTimerMax = 15f;
   private float heartBeatTimer;

   private void Awake()
   {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      heartBeatTimer = heartBeatTimerMax;
      InitializeUnityAuthentication();
   }
   
   private async void InitializeUnityAuthentication()
   {
      if (UnityServices.State != ServicesInitializationState.Initialized)
      {
         InitializationOptions initializationOptions = new InitializationOptions();
         await UnityServices.InitializeAsync(initializationOptions);
         await AuthenticationService.Instance.SignInAnonymouslyAsync();
      }
   }

   private void Update()
   {
      SendHeartBeat();
   }

   private void SendHeartBeat()
   {
      if (isLobbyHost())
      {
         heartBeatTimer -= Time.deltaTime;
         if (heartBeatTimer < 0)
         {
            heartBeatTimer = heartBeatTimerMax;
            LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
         }
      }
   }

   private bool isLobbyHost()
   {
      return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
   }

   private async Task<Allocation> AllocateRelay()
   {
      try
      {
         Allocation allocation =
            await RelayService.Instance.CreateAllocationAsync(KitchenGameMultiPlayer.Instance.MAX_PLAYER_NUMBER - 1);

         return allocation;
      }
      catch (RelayServiceException e)
      {
         Debug.Log(e);
         return default;
      }
   }

   private async Task<JoinAllocation> JoinRelay(string joinCode)
   {
      try
      {
         JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
         return joinAllocation;
      }
      catch (RelayServiceException e)
      {
         Debug.Log(e);
         return default;
      }
   }
   
   
   private async Task<string> GetRelayJoinCode(Allocation allocation)
   {
      try
      {
         string relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
         return relayCode;
      }
      catch (RelayServiceException e)
      {
         Debug.Log(e);
         return default;
      }
   }
   
   public async void CreateLobby(string lobbyName, bool isPrivate)
   {
      try
      {
         
         Allocation allocation = await AllocateRelay();
         NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
            allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData);
         
         string joinCode = await GetRelayJoinCode(allocation);

         joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,
            KitchenGameMultiPlayer.Instance.MAX_PLAYER_NUMBER, new CreateLobbyOptions()
            {
               IsPrivate = isPrivate,
               Data = new Dictionary<string, DataObject>
               {
                  { RELAY_JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
               }
            });
         
           KitchenGameMultiPlayer.Instance.StartHost();
           Loader.Network_LoadSceneMode(Loader.Scene.CharacterSelectScene);
      }
      catch (LobbyServiceException e)
      {
         Debug.Log(e);
      }
      
   }

   public Lobby GetJoinedLobby()
   {
      return joinedLobby;
   }
   
   public async void JoinedLobbyByCode(string lobbyCode)
   {
      try
      {
         joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

         string joinCode = joinedLobby.Data[RELAY_JOIN_CODE].Value;
         
         JoinAllocation joinAllocation = await JoinRelay(joinCode);
         
         NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
            joinAllocation.RelayServer.IpV4,
            (ushort)joinAllocation.RelayServer.Port,
            joinAllocation.AllocationIdBytes,
            joinAllocation.Key,
            joinAllocation.ConnectionData,
            joinAllocation.HostConnectionData);
         
         
         KitchenGameMultiPlayer.Instance.StartClient();
      }
      catch (LobbyServiceException e)
      {
         Debug.Log(e);  
      }
   }
   

   public async void DestroyLobby()
   {
      try
      {
         if (joinedLobby != null)
         {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
            joinedLobby = null;
         }
      }
      catch (LobbyServiceException e)
      {
         Debug.Log(e);
      }
   }

   public async void LeaveLobby()
   {
      if (joinedLobby != null)
      {
         try
         {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            joinedLobby = null;
         }
         catch (LobbyServiceException e)
         {
            Debug.Log(e);
         }
      }
   }
}
