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

    private void Update()
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormalised();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        transform.position += moveDir * moveSpeed * Time.deltaTime;

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
