using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenGameMultiPlayer : NetworkBehaviour
{

    public int MAX_PLAYER_NUMBER = 4;

    [SerializeField] private _KitchenObjectSOList KitchenObjectSoList;
    [SerializeField] private List<Color> playerColors;
    private Dictionary<ulong, bool> playersIsReady;
    private NetworkVariable<int> playersNumber = new NetworkVariable<int>(0);
    private NetworkList<PlayerData> playersData;
    public event Action OnClientTryToConnect;
    public event Action OnClientFailToConnect;
    public event Action OnPlayerDataListChanged;
    public event Action OnPlayerIsReady;
    
    public static KitchenGameMultiPlayer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        playersIsReady = new Dictionary<ulong, bool>();
        DontDestroyOnLoad(gameObject);
        playersData = new NetworkList<PlayerData>();
        playersData.OnListChanged += ListChanged;
    }

    private void ListChanged(NetworkListEvent<PlayerData> changeevent)
    {
        OnPlayerDataListChanged?.Invoke();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientConnectedCallback += PlayerJoinedCharacterSelectionLobby;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandlePlayerDisconnectFromCharacterSelectScene;
        NetworkManager.Singleton.StartHost();
    }

    private void HandlePlayerDisconnectFromCharacterSelectScene(ulong clientId)
    {
        for (int i = 0; i < playersData.Count; i++)
        {
            PlayerData playerData = playersData[i];
            if (playerData.clientid == clientId)
            {
                playersData.RemoveAt(i);
                playersNumber.Value--;
                break;
            }
        }
    }

    private void PlayerJoinedCharacterSelectionLobby(ulong clientid)
    {
        playersData.Add(new PlayerData()
        {
            clientid = clientid,
            colorid = GetFirstUnusedColorId()
        });
    }

    private void OnClientConnected(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
       
        if (SceneManager.GetActiveScene().name == Loader.Scene.GameScene.ToString())
        {
            response.Approved = false;
            response.Reason = "Game already Started";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_NUMBER)
        {
            response.Approved = false;
            response.Reason = "Game is Full";
            return;
        }
        
        response.Approved = true;
    }

    public void StartClient()
    {
       OnClientTryToConnect?.Invoke();
       NetworkManager.Singleton.OnClientDisconnectCallback += PlayerDisconnected;
       NetworkManager.Singleton.StartClient();
    }

    private void PlayerDisconnected(ulong obj)
    {
        OnClientFailToConnect?.Invoke();
    }


    public void CreateKitchenObject(KitchenObjectSO kitchenObjectSo, IKitchenObjectParent kitchenObjectParent)
    {
        CreateTheObjectServerRpc(GetKitchenObjectIndex(kitchenObjectSo),kitchenObjectParent.GetNetworkObject());
    }

    public void DestroyKitchenObject(NetworkObjectReference kitchenObjectNo)
    {
        DestroyKitchenObjectServerRpc(kitchenObjectNo);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateTheObjectServerRpc(int index, NetworkObjectReference networkObject_kitchenObjectParent)
    {
        networkObject_kitchenObjectParent.TryGet(out NetworkObject parentNetworkObject);

        IKitchenObjectParent kitchenObjectParent = parentNetworkObject.GetComponent<IKitchenObjectParent>();

        if (kitchenObjectParent.HasKitchenObject())
        {
            return;
        }
        
        Transform kitchenObjectT = Instantiate(GetKitchenObjectSoFromIndex(index).prefap);

        NetworkObject kitchenObjectNetworkObject = kitchenObjectT.GetComponent<NetworkObject>();
        kitchenObjectNetworkObject.Spawn(true);
        
        
        KitchenObject kitchenObject = kitchenObjectT.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenParent(kitchenObjectParent);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectNo)
    {
        kitchenObjectNo.TryGet(out NetworkObject kitchenObjectN);
        
        if (kitchenObjectN == null)
        {
            return;
        }
        
        KitchenObject kitchenObject = kitchenObjectN.GetComponent<KitchenObject>();

        
        
        ClearKitchenObjectClientRpc(kitchenObject.GetKitchenParent().GetNetworkObject());
        kitchenObject.DestroySelfServerSide();
    }

    [ClientRpc]
    private void ClearKitchenObjectClientRpc(NetworkObjectReference kitchenObjectParentNo)
    {
        kitchenObjectParentNo.TryGet(out NetworkObject kitchenObjectParentN);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentN.GetComponent<IKitchenObjectParent>();
        kitchenObjectParent.ClearKitchenObject();
    }

    public int GetKitchenObjectIndex(KitchenObjectSO kitchenObjectSo)
    {
        return KitchenObjectSoList.kitchenObjectSoList.IndexOf(kitchenObjectSo);
    }

    public KitchenObjectSO GetKitchenObjectSoFromIndex(int index)
    {
        return KitchenObjectSoList.kitchenObjectSoList[index];
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerAsReadyServerRpc(bool isReady,ServerRpcParams clientId = default)
    {
        playersIsReady[clientId.Receive.SenderClientId] = isReady;
        SetPlayerAsReadyClientRpc(isReady,clientId.Receive.SenderClientId);
        if (isReady)
        {
            playersNumber.Value++;
        }
        else
        {
            playersNumber.Value--;
        }
        bool startGame = true;
        foreach (ulong CI in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playersIsReady.ContainsKey(CI) || !playersIsReady[CI])
            {
                startGame = false;
            }
        }

        if (startGame)
        {
            KitchenGameLobby.Instance.DestroyLobby();
            Loader.Network_LoadSceneMode(Loader.Scene.GameScene);
        }
    }

    [ClientRpc]
    private void SetPlayerAsReadyClientRpc(bool isReady, ulong clientId)
    {
        playersIsReady[clientId] = isReady;
        OnPlayerIsReady?.Invoke();
    }

    public int GetPlayersNumber()
    {
        return playersNumber.Value;
    }

    public bool isPlayerConnected(int playerIndex)
    {
        return playersData.Count > playerIndex;
    }

    public PlayerData GetPlayerDataFromIndex(int playerIndex)
    {
        return playersData[playerIndex];
    }

    public bool isPlayerReady(ulong clientId)
    {
        return playersIsReady.ContainsKey(clientId) && playersIsReady[clientId];
    }

    public Color GetPlayerColor(int playerIndex)
    {
        return playerColors[playerIndex];
    }

    public PlayerData GetPlayerData()
    {
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }

    public PlayerData GetPlayerDataFromClientId(ulong clientId)
    {
        foreach (PlayerData playerData in playersData)
        {
            if (playerData.clientid == clientId)
            {
                return playerData;
            }
        }

        return default;
    }

    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < playersData.Count; i++)
        {
            if (playersData[i].clientid == clientId)
            {
                return i;
            }
        }

        return -1;
    }

    private bool isColorUsed(int colorId)
    {
        foreach (PlayerData playerData in playersData)
        {
            if (playerData.colorid == colorId)
            {
                return true;
            }
        }

        return false;
    }
    
    public void ChangePlayerColor(int colorId)
    {
        ChangePlayerColorServerRpc(colorId);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerColorServerRpc(int colorId, ServerRpcParams serverRpcParams = default)
    {
        if (isColorUsed(colorId))
        {
            return;
        }

        int playerIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playersData[playerIndex];

        playerData.colorid = colorId;

        playersData[playerIndex] = playerData;
    }

    private int GetFirstUnusedColorId()
    {
        for (int i = 0; i < playerColors.Count; i++)
        {
            if (!isColorUsed(i))
            {
                return i;
            }
        }

        return default;
    }
    
}
