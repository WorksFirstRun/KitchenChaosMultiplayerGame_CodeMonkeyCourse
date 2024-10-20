using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour , IKitchenObjectParent
{
    public static event EventHandler onPlayerSpawn; 
    
    public event EventHandler<OnSelectedCounterChangeArgs> OnSelectedCounterChange;
    
    public event EventHandler OnPlayPickUpSound;
    
    
    [SerializeField] private float speed;
    
    [SerializeField] private Transform objectHoldPoint;

    [SerializeField] private LayerMask collisionMask;

    [SerializeField] private List<Vector3> spawnPositions;

    [SerializeField] private PlayerColorAndVisual playerVisual;
    
    private bool walking;
    
    /*private float playerHeight = 2f;*/
    
    private float playerRadius = 0.7f;
    
    private Vector3 lastInteractDir;
    
    private BaseCounterScript selectedCounter;
    
    private KitchenObject kitchenObject;
    
    
    public static PlayerMovement LocalInstance { get; private set; } // singletone pattern
    
    
    
    public class OnSelectedCounterChangeArgs : EventArgs
    {
        public BaseCounterScript selectedCounter;
    }

    
    
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }

        int playerIndex = KitchenGameMultiPlayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId);
        transform.position = spawnPositions[playerIndex];
        onPlayerSpawn?.Invoke(this,EventArgs.Empty);

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += PlayerDisconnect;
        }
        
        base.OnNetworkSpawn();
    }

    private void PlayerDisconnect(ulong obj)
    {
        if (HasKitchenObject())
        {
            GetKitchenObject().DestroySelf();
        }
    }

    private void Start()
    {
        TakeInput.Instance.onInteractAction += HandleInteractions;
        TakeInput.Instance.onCuttingAction += HandleCuttingAction;

        PlayerData playerData = KitchenGameMultiPlayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerColor(KitchenGameMultiPlayer.Instance.GetPlayerColor(playerData.colorid));
    }

    private void HandleCuttingAction()
    {
        if (GameManager.Instance.IsGameRunning())
        {
           if (selectedCounter != null)
           {
               selectedCounter.CutInteraction();
           } 
        }
    }

    
    
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        MoveThePlayer();
        HandleCollision();
    }

    private void HandleInteractions()
    {
        if (GameManager.Instance.IsGameRunning())
        {
            if (selectedCounter != null)
            {
                selectedCounter.Interact(this);
            }
        }
    }

    private void HandleCollision() // Custom Collision Function using Physics.Raycast  
    {
        Vector2 directionV2 = TakeInput.Instance.GetDirectionInputNormalized();

        Vector3 directionV3 = new Vector3(directionV2.x, 0, directionV2.y);

        if (directionV3 != Vector3.zero)
        {
            lastInteractDir = directionV3;
        }

        float playerInteractionDistance = 2f;
        RaycastHit collidedObject;
        if (PlayerCollision(lastInteractDir,playerInteractionDistance,out collidedObject))
        { 
            if(collidedObject.transform.TryGetComponent<BaseCounterScript>(out BaseCounterScript counter))
            {
                if (counter != null)
                {
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    void MoveThePlayer()
    {
        Vector2 direction = TakeInput.Instance.GetDirectionInputNormalized();
        float distance = speed * Time.deltaTime;
        Vector3 directionV3 = new Vector3(direction.x, 0, direction.y);
        if (!CanPlayerMove(directionV3, distance))
        {
            Vector3 directionX = new Vector3(directionV3.x, 0, 0);
            if (CanPlayerMove(directionX, distance) && directionX.x != 0)
            {
                directionV3 = directionX;
            }
            else
            {
                Vector3 directionY = new Vector3(0, 0, directionV3.z);
                if (CanPlayerMove(directionY, distance) && directionY.z != 0)
                {
                    directionV3 = directionY;
                }
            }
        }

        directionV3 *= distance;
        if (CanPlayerMove(directionV3, distance))
        {
            transform.Translate(directionV3, Space.World);
        }

        walking = directionV3 != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward,directionV3, distance);
    }

   

    private void SetSelectedCounter(BaseCounterScript selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChange?.Invoke(this ,new OnSelectedCounterChangeArgs
        {
            selectedCounter = selectedCounter
        });
    }
    
    public Transform GetParentHoldPoint()
    {
        return objectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject newKitchenObject)
    {
        kitchenObject = newKitchenObject;
        if (kitchenObject != null)
        {
            OnPlayPickUpSound?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    
    public bool PlayerState()
    {
        return walking;
    }

    private bool CanPlayerMove(Vector3 direction, float distance)
    {
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, direction.normalized, Quaternion.identity, distance,collisionMask);
        return canMove;
    }
    
    private bool PlayerCollision(Vector3 direction,float interactDistance, out RaycastHit c)
    {
        bool isColliding = Physics.Raycast(transform.position, direction,out RaycastHit collidedObject ,interactDistance);
        c = collidedObject;
        return isColliding;
    }

    public static void ResetStaticData()
    {
        onPlayerSpawn = null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
    
   
}
