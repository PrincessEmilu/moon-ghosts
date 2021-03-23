using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera = null;

    //Movement Related values
    [SerializeField] private float mouseSensitivity = 3.5f;
    [SerializeField] private float movementSpeed = 8.0f;
    [SerializeField] private float gravity = -15.0f;
    [SerializeField] [Range(0.0f, 0.5f)] private float movementSmoothDuration = 0.3f;

    //Look or Rotation related values
    [SerializeField] [Range(0.0f, 0.5f)] private float lookSmoothDuration = 0.03f;

    private float cameraPitch = 0.0f;

    //Track Gravity
    private float velocityY = 0.0f;

    private CharacterController characterController = null;

    //Smoothening Related values
    private Vector2 currentSmoothedDirection = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;
    private Vector2 currentSmoothedMouseInput = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();

        //toDo Decouple from this controller script.
        DisableCursor();   
    }

    private static void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //toDo Decouple Cursor locking from this controller script. 
        ProcessUnlockCursorInput();

        ProcessPlayerLook();
        ProcessPlayerMovement();
    }

    private void ProcessPlayerMovement()
    {
        Vector2 targetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        targetDirection.Normalize();

        currentSmoothedDirection = Vector2.SmoothDamp(currentSmoothedDirection, targetDirection, ref currentDirVelocity, movementSmoothDuration);

        //Gravity Related
        if (characterController.isGrounded)
        {
            velocityY = 0.0f;
        }
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentSmoothedDirection.y + transform.right * currentSmoothedDirection.x);

        velocity *= movementSpeed;

        velocity += Vector3.up * velocityY;

        characterController.Move(velocity * Time.deltaTime);

    }

    private void ProcessPlayerLook()
    {
        Vector2 targetMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentSmoothedMouseInput = Vector2.SmoothDamp(currentSmoothedMouseInput, targetMouseInput, ref currentMouseDeltaVelocity, lookSmoothDuration);

        cameraPitch -= currentSmoothedMouseInput.y * mouseSensitivity;

        //Clamping pitch so that we don't run into rotation anomalies.
        cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);

        playerCamera.transform.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(transform.up * currentSmoothedMouseInput.x * mouseSensitivity);


    }

    private void ProcessUnlockCursorInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
