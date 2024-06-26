using System.Collections;

using System.Collections.Generic;

using UnityEngine;



[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform playerCameraParent;
    
    [SerializeField] private Camera camera;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpPower = 4f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float sensivity = 2f;
    [SerializeField] private float lookXLimit = 30f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX;
    private CharacterController characterController;
    Vector2 rotation = Vector2.zero;
    
    private bool canMove = true;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rotation.y = transform.eulerAngles.y;
    }
    

    void Update()

    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }


        characterController.Move(moveDirection * Time.deltaTime);
        
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * sensivity;
            rotation.x += -Input.GetAxis("Mouse Y") * sensivity;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensivity, 0);
        }
    }

}