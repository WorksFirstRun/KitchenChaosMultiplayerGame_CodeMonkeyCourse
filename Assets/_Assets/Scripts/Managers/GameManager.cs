using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    
    public static GameManager Instance { get; private set; }
    public event EventHandler onStateChanged;
    public event EventHandler onGamePause;
    public event EventHandler onGameUnPause;

    [SerializeField] private Transform playerPrefab;
    
    public enum State{
        WaitingToStart,
        CountDownToStart,
        GameRunning,
        GameOver,
    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private NetworkVariable<float> countDownTimer = new NetworkVariable<float>(3);
    private NetworkVariable<float> gameRunningTimer = new NetworkVariable<float>(0);
    private NetworkVariable<int> currentNumberofPlayers = new NetworkVariable<int>(0);
    private NetworkVariable<bool> isAllPlayersSpawned = new NetworkVariable<bool>(false);
    private float gameRunningMaxTimer = 180f;
    private bool isGamePaused;
    private int playerNumber;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    { 
        TakeInput.Instance.onPauseGame += Instance_OnPauseGame;
        playerNumber = KitchenGameMultiPlayer.Instance.GetPlayersNumber();
        PlayerHasJoined();
    }


    private void PlayerHasJoined()
    {
        PlayerJoinedServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlayerJoinedServerRpc()
    {
        currentNumberofPlayers.Value++;
        if (playerNumber == currentNumberofPlayers.Value)
        {
            foreach (ulong CI in NetworkManager.Singleton.ConnectedClientsIds)
            {
                Transform player = Instantiate(playerPrefab);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(CI,true);
            }

            isAllPlayersSpawned.Value = true;
        }
    }
    
    
    public override void OnNetworkSpawn()
    {
         state.OnValueChanged += OnValueChanged;
    }
    
     private void OnValueChanged(State previousvalue, State newvalue)
     {
         onStateChanged?.Invoke(this,EventArgs.Empty);
     }
     
    private void Instance_OnPauseGame()
    {
        PauseGame();
    }


    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        
        if (isAllPlayersSpawned.Value && state.Value == State.WaitingToStart)
        {
            state.Value = State.CountDownToStart;
            onStateChanged?.Invoke(this,EventArgs.Empty);
        }
        
        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountDownToStart:
                countDownTimer.Value -= Time.deltaTime;
                if (countDownTimer.Value <= 0)
                {
                    state.Value = State.GameRunning;
                }
                break;
            case State.GameRunning:
                gameRunningTimer.Value += Time.deltaTime;
                if (gameRunningTimer.Value >= gameRunningMaxTimer)
                {
                    state.Value = State.GameOver;
                }
                break;
            case State.GameOver:
                // fire the event here 
                break;
        }
    }

    public bool IsGameRunning()
    {
        return state.Value == State.GameRunning;
    }

    public bool IsGameCountDownToStart()
    {
        return state.Value == State.CountDownToStart;
    }

    public float GetCountDownTimer()
    {
        return countDownTimer.Value;
    }

    public bool IsGameWaitingToStart()
    {
        return state.Value == State.WaitingToStart;
    }

    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }

    public float GetGameRunningTimerNormalized()
    {
        return gameRunningTimer.Value / gameRunningMaxTimer;
    }

    public void PauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            onGamePause?.Invoke(this , EventArgs.Empty);
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 1f;
            onGameUnPause?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ChangeTimer(float number)
    {
        ChangeTimerServerRpc(number);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeTimerServerRpc(float number)
    {
         gameRunningTimer.Value += number;
         gameRunningTimer.Value = Mathf.Clamp(gameRunningTimer.Value, 0, gameRunningMaxTimer);
    }
    
    
}
