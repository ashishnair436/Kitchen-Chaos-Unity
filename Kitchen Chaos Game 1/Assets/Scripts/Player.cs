using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{


    [SerializeField]
    private float moveSpeed = 7f;

    [SerializeField]
    private float rotateSpeed = 10f;

    [SerializeField]
    private GameInput gameInput;

    private bool isWalking;

    private float playerRadius = .7f;

    private float playerHeight = 2f;


    private void Update()
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormalised();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight ,playerRadius, moveDir , moveDistance);


        if (!canMove)
        {
            // meaning we cant move right now so try to attempt movement in only X and only z direction 
            // this is for when player wants to hug the wall and move diagonally to the collision object in front of him

            // as we cant move in moveDir we will split moveDir.x and moveDir.z and see if we can move now 

            // Attempt only X movement

            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;

            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX , moveDistance);

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

                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

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
}
