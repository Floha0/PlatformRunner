using System;
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
    private Vector2 rotation = Vector2.zero;
    
    private bool canMove = true;


    [SerializeField] private float maxRunTime = 5f;
    public float runCooldownTime = 3.0f; // Time the player must wait before running again
    private float runTimeRemaining;
    private float runCooldownRemaining = 0f;
    
    // Camera Transform Variables
    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private float maxDistance = 4.0f;
    [SerializeField] private float smooth = 20.0f;
    private Vector3 dollyDir;
    private float distance;

    private void Awake()
    {
        dollyDir = camera.transform.localPosition.normalized;
        distance = camera.transform.localPosition.magnitude;
    }

    void Start()
    {
        runTimeRemaining = maxRunTime;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rotation.y = transform.eulerAngles.y;
    }
    

    void Update()
    {
        CameraCollision();
        
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        bool isRunning = Input.GetKey(KeyCode.LeftShift)&& runTimeRemaining > 0 && runCooldownRemaining <= 0;
        
        // Set current speed and direction
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        
        // Running time management
        if (isRunning)
        {
            runTimeRemaining -= Time.deltaTime;
        }
        else if (runTimeRemaining < maxRunTime)
        {
            runTimeRemaining += Time.deltaTime; // Recover running time if not running
        }
        
        UpdateRunCooldown();

        Debug.Log(runTimeRemaining);
        
        
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Jump
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        
        // Apply gravity 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        
        // Move
        characterController.Move(moveDirection * Time.deltaTime);
        
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * sensivity;
            rotation.x += -Input.GetAxis("Mouse Y") * sensivity;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit); // For the rotation limit on x axis
            
            // Rotate the camera
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensivity, 0);
        }
    }
    
    
    void UpdateRunCooldown()
    {
        if (runTimeRemaining <= 0 && runCooldownRemaining <= 0)
        {
            runCooldownRemaining = runCooldownTime; // Start cooldown
        }

        if (runCooldownRemaining > 0)
        {
            runCooldownRemaining -= Time.deltaTime;
            if (runCooldownRemaining < 0)
            {
                runCooldownRemaining = 0;
                runTimeRemaining = maxRunTime; // Reset run time after cooldown
            }
        }
    }

    void CameraCollision()
    {
        Vector3 back = camera.transform.TransformDirection(Vector3.back);
        if (Physics.Raycast(camera.transform.position, back, out RaycastHit hit, 2))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        camera.transform.localPosition  = Vector3.Lerp(camera.transform.localPosition , dollyDir * distance, Time.deltaTime * smooth);
        Debug.DrawRay(camera.transform.position, back * 2, Color.red);
    }
}