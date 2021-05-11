using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class FPSController : MonoBehaviour, IDamageable<float>
{
    #region Fields
    [SerializeField] private Camera playerCamera = null;
    private Transform playerCamTransform = null;

    //Movement Related values
    [SerializeField] private float mouseSensitivity = 3.5f;
    [SerializeField] private float movementSpeed = 8.0f;
    [SerializeField] private float gravity = -10.0f;
    [SerializeField] [Range(0.0f, 0.3f)] private float movementSmoothDuration = 0.2f;

    //Jetpack things
    [SerializeField] private float minThrust = 0.95f;
    [SerializeField] private float maxThrust = 2.0f;
    [SerializeField] private float currentThrust = 2.0f;

    // Dash cooldown
    [SerializeField] private float maxDashCoolDown = 2.0f;
    [SerializeField] private float dashCD = 0.0f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashVelocityMultiplier = 1.5f;
    private Vector3 dash = Vector3.zero;
    private bool dashCDSoundPRoc = true;

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

    [SerializeField] private float health = 3;

    [FMODUnity.EventRef]
    public string dashEvent;

    [FMODUnity.EventRef]
    public string cooldownEvent;

    [FMODUnity.EventRef]
    public string damagedEvent;

    [FMODUnity.EventRef]
    public string walkingEvent;
    FMOD.Studio.EventInstance walkingState;

    public bool walkingWithFMOD = false;

    [FMODUnity.EventRef]
    public string flyingEvent;
    FMOD.Studio.EventInstance flyingState;

    public bool alreadyFlying;

    /*
    //aim assist related
    public float detectionRadius = 20f;

    public LayerMask targetLayer;
    public LayerMask obstacleLayer;

    public float fovAngle = 40f;
    */
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        playerCamTransform = playerCamera.transform;
        walkingState = FMODUnity.RuntimeManager.CreateInstance(walkingEvent);
        flyingState = FMODUnity.RuntimeManager.CreateInstance(flyingEvent);

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
        ProcessUnlockCursorInput();

        ProcessPlayerLook();
        ProcessPlayerMovement();


    }

    /*
    private void ProcessNormalControl()
    {
        if(MainMenu.AimAssistON == true)
        {
            if (Input.GetMouseButtonDown(1))
            {

                controlState = ControlState.aimassit;
            }

            //Debug.Log("AIM ASSIST ON");
        }  

        ProcessPlayerLook();
        ProcessPlayerMovement();
    }
    */

    private void ProcessPlayerMovement()
    {
        Vector2 targetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        targetDirection.Normalize();

        currentSmoothedDirection = Vector2.SmoothDamp(currentSmoothedDirection, targetDirection, ref currentDirVelocity, movementSmoothDuration);

        ProcessJetpack();

        Vector3 velocity = (transform.forward * currentSmoothedDirection.y + transform.right * currentSmoothedDirection.x);

        // movement speed is faster on the ground and slower in the air
        if (characterController.isGrounded)
        {
            velocity *= movementSpeed;
        } else
        {
            velocity *= 0.8f * movementSpeed;
        }

        // play auditory indicator for dash being ready, only plays once & then switches the bool

        if (dashCD <= 0 && dashCDSoundPRoc == false)
        {
            FMODUnity.RuntimeManager.PlayOneShot(cooldownEvent, transform.position);
            dashCDSoundPRoc = true;
        }

        // dash is strictly horizontal, so process before adding vertical velocity

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCD <= 0)
        {
            dash = ProcessDash(velocity);
            FMODUnity.RuntimeManager.PlayOneShot(dashEvent, transform.position);
            dashCDSoundPRoc = false;
        }
        // For a specified duration, add the dash vector to velocity
        if (dashCD >= maxDashCoolDown - dashDuration)
        {
            velocity += dash;
        }
        dashCD -= Time.deltaTime;

        velocity += Vector3.up * velocityY;

        // play walking sound effect if moving
        WalkingSounds();

        characterController.Move(velocity * Time.deltaTime);

    }

    private void ProcessJetpack()
    {
        //Gravity Related
        if (characterController.isGrounded)
        {
            velocityY = 0.0f;
            currentThrust = maxThrust;
        }
        velocityY += gravity * Time.deltaTime;

        // Jumping / Jetpack
        if (Input.GetKey(KeyCode.Space))
        {
            if(!alreadyFlying)
            {
                flyingState.start();
                alreadyFlying = true;
            }
            
            velocityY += -currentThrust * gravity * Time.deltaTime;
            if (currentThrust > minThrust) { currentThrust -= 0.002f; }

            // first time the space bar registers, you want to start the event.
            // first time space bar is released, you want to stop the event.
        }
        else
        {
            if (alreadyFlying)
            {
                flyingState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                alreadyFlying = false;
            }
        }
    }

    private Vector3 ProcessDash(Vector3 velocity)
    {
        dashCD = maxDashCoolDown;
        return velocity * dashVelocityMultiplier;
    }

    private void ProcessPlayerLook()
    {
        Vector2 targetMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentSmoothedMouseInput = Vector2.SmoothDamp(currentSmoothedMouseInput, targetMouseInput, ref currentMouseDeltaVelocity, lookSmoothDuration);

        cameraPitch -= currentSmoothedMouseInput.y * mouseSensitivity;

        //Clamping pitch so that we don't run into rotation anomalies.
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

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

    /*
    public Transform GetClosestUnobstructedTarget()
    {
        List<Collider> unobstructedColliders = new List<Collider>();

        Collider[] colliders = Physics.OverlapSphere(playerCamTransform.position, detectionRadius, targetLayer);

        foreach (Collider collider in colliders)
        {

            Vector3 targetDir = (collider.transform.position - playerCamTransform.position).normalized;

            float angle = Vector3.Angle(playerCamTransform.forward, targetDir);

            if (Mathf.Abs(angle) < fovAngle)
            {

                float distanceToTarget = Vector3.Distance(playerCamTransform.position, collider.transform.position);

                if (!Physics.Raycast(playerCamTransform.position, targetDir, distanceToTarget, obstacleLayer))
                {
                    //This raycast determines if something is blocking our desired target or not.
                    unobstructedColliders.Add(collider);
                }
            }

        }
        if (unobstructedColliders.Count > 0)
        {
            //sorting our targest by distance.
            unobstructedColliders = unobstructedColliders.OrderBy(x => Vector3.Distance(x.transform.position, playerCamTransform.position)).ToList();

            //This is the nearest target.
            Collider closestCollider = unobstructedColliders[0];

            return closestCollider.transform;
        }
        else
        {
            Debug.Log("Nothing to target");
            return null;
        }

    }

    public void ActivateAimAssist()
    {
        Transform target = GetClosestUnobstructedTarget();
        if (target)
        {
            Vector3 targetDirection = (target.position - playerCamTransform.position).normalized;

            Quaternion lookRoation = Quaternion.LookRotation(targetDirection, playerCamTransform.up);

            float targetCameraPitch = lookRoation.eulerAngles.x;
            float targetYaw = lookRoation.eulerAngles.y;

            targetCameraPitch = (targetCameraPitch > 180) ? targetCameraPitch - 360 : targetCameraPitch;
            targetYaw = (targetYaw > 180) ? targetYaw - 360 : targetYaw;

            playerCamTransform.localEulerAngles = new Vector3(targetCameraPitch, playerCamTransform.localEulerAngles.y, playerCamTransform.localEulerAngles.z);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, targetYaw, transform.localEulerAngles.z);

            cameraPitch = targetCameraPitch;
        }
        controlState = ControlState.fps;

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerCamera.transform.position, detectionRadius);
    }
    */

    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        FMODUnity.RuntimeManager.PlayOneShot(damagedEvent, transform.position);

        if (health <= 0 )
        {
            // transform.position = new Vector3(32.0f, 1.0f, 31.0f);
            walkingState.release();
            flyingState.release();
            walkingState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            flyingState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            health = 3.0f;
            
            // end screen here
            // 4 is the int for try again screen
            SceneManager.LoadScene(4);
        }
    }

    private void WalkingSounds()
    {
        
        if (characterController.isGrounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            if(!walkingWithFMOD)
            {
                walkingState.start();
                walkingWithFMOD = true;
            }
        }
        else
        {
            walkingState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            walkingWithFMOD = false;
        }
        
    }
}
