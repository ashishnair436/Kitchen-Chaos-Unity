using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour , IKitchenObjectParent
{
    // the below line is called a C# property and is used in something called singleton pattern 

    public static Player Instance { get;  private set; }



    /* the below lines of code is similar to the single line of code above 
     * 
     * public static Player instanceField;
     * public static Player GetInstanceField(){
     *  return instanceField;
     *  }
     *  
     *  public static Player SetInstanceField(Player instanceField){
     *     Player.instanceField = instanceField;
     *     }
     *     
     *     */

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }





    [SerializeField]
    private float moveSpeed = 7f;

    [SerializeField]
    private float rotateSpeed = 10f;

    [SerializeField]
    private GameInput gameInput;

    [SerializeField]
    private LayerMask countersLayerMask;

    [SerializeField]
    private Transform kitchenObjectHoldPoint;



    private bool isWalking;

    private float playerRadius = .7f;

    private float playerHeight = 2f;

    private Vector3 lastInteractDir;


    private BaseCounter selectedCounter;


    private KitchenObject kitchenObject;


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one player instance");
        }

        Instance = this;
    }



    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {

        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }

    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
       
    }

    private void HandleInteractions()
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormalised();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir ;
        }

        float interactDistance = 2f;
        
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit , interactDistance , countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //Has ClearCounter 

                /* tryget component is same as getcomponent but you dont need to handle null exception 
                for example same code in get component will be as

                 ClearCounter clearCounter = raycastHit.transform.GetComponent<ClearCounter>();
                if(clearCounter!= null){
                Has Clearcounter
                }
                
                 */
                // clearCounter.Interact();

                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);

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

    private void HandleMovement()
    {


        Vector2 inputVector = gameInput.GetMovementVectorNormalised();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);


        if (!canMove)
        {
            // meaning we cant move right now so try to attempt movement in only X and only z direction 
            // this is for when player wants to hug the wall and move diagonally to the collision object in front of him

            // as we cant move in moveDir we will split moveDir.x and moveDir.z and see if we can move now 

            // Attempt only X movement

            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;

            canMove = moveDir.x!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //canMove only on the x axis
                moveDir = moveDirX;
            }

            else
            {
                // meaning now we cannot move only on x axis so we attempt on Z axis

                // Attempt on Z axis

                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;

                canMove = moveDir.z!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    //means we can move only on the z 
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction
                }

            }

        }


        if (canMove)
        {
            // this canmove is for standard movement in any direction and not for wall hugging
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }



        // below code is to see if we are walking or not to send the value to the animator
        isWalking = moveDir != Vector3.zero;


        // this below is janky so we will use slerp to smooth out the rotation
        // transform.forward = moveDir;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }


    public bool IsWalkingBro()
    {
        return isWalking;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs{
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
