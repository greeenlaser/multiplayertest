using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_Movement : MonoBehaviour
{
    [Header("Player default values")]
    public float walkSpeed;
    public float crouchSpeed;
    public float sprintSpeed;
    public float jumpHeight;
    public float crouchHeight;
    public float maxStamina;
    public float staminaRecharge;
    public float staminaCooldownLimit;
    [SerializeField] private Vector3 cameraFullHeight;
    [SerializeField] private Vector3 cameraCrouchHeight;

    //public but hidden variables
    public bool canMove;
    public bool canSprint;
    public bool isSprinting;
    public bool canJump;
    public bool isJumping;
    public bool canCrouch;
    public bool isCrouching;
    public bool isGrounded;
    public float currentStamina;
    public Vector3 velocity;
    [HideInInspector] public CharacterController controller;
    private Camera PlayerCamera;
    private GameObject checkSphere;
    private PhotonView view;

    //private variables
    private bool startStaminaRechargeCooldown;
    private bool cooldownFinished;
    private float staminaCooldown;
    private readonly float gravity = -9.81f;
    private float originalHeight;
    private float currentSpeed;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        view.RequestOwnership();

        foreach (Transform child in transform)
        {
            if (child.name == "checkSphere")
            {
                checkSphere = transform.gameObject;
            }
        }
        controller = GetComponent<CharacterController>();
        PlayerCamera = transform.GetComponentInChildren<Camera>();

        currentSpeed = walkSpeed;
        currentStamina = maxStamina;
        canMove = true;
        canSprint = true;
        canCrouch = true;
        canJump = true;

        originalHeight = controller.height;
        PlayerCamera.transform.localPosition = cameraFullHeight;
    }

    private void Update()
    {
        if (view.IsMine)
        {
            //check if player is grounded
            if (Physics.CheckSphere(checkSphere.transform.position,
                                    0.4f,
                                    LayerMask.NameToLayer("Ground")))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

            if (velocity.y < 0
                && isGrounded)
            {
                velocity.y = -2f;
            }

            if (!isGrounded)
            {
                velocity.y += gravity * Time.deltaTime * 4f;
            }

            if (canMove)
            {
                //movement input
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");

                Vector3 move = transform.right * x + transform.forward * z;
                move = Vector3.ClampMagnitude(move, 1);

                //first movement update based on speed and input
                controller.Move(currentSpeed * Time.deltaTime * move);

                //final movement update based on velocity
                controller.Move(velocity * Time.deltaTime);

                //get all velocity of the controller
                Vector3 horizontalVelocity = transform.right * x + transform.forward * z;

                //sprinting
                if (Input.GetKeyDown(KeyCode.LeftShift)
                    && canSprint
                    && currentStamina >= 0.1f)
                {
                    staminaCooldown = 0;
                    isSprinting = true;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    isSprinting = false;
                }
                if (isSprinting
                    && horizontalVelocity.magnitude > 0.3f)
                {
                    //Debug.Log("Player is sprinting!");

                    currentSpeed = sprintSpeed;
                    currentStamina -= 8 * Time.deltaTime;

                    StaminaCooldownFinished();

                    if (isCrouching)
                    {
                        isCrouching = false;

                        controller.height = originalHeight;

                        PlayerCamera.transform.localPosition = cameraFullHeight;
                    }

                    if (currentStamina <= 0.1f)
                    {
                        isSprinting = false;
                    }
                }
                //force-disables sprinting if the player is no longer moving but still holding down sprint key
                else if (isSprinting
                         && horizontalVelocity.magnitude < 0.3f)
                {
                    isSprinting = false;
                }
                else if (!isSprinting)
                {
                    if (!isCrouching)
                    {
                        currentSpeed = walkSpeed;
                    }

                    //recharge stamina
                    if (currentStamina < maxStamina)
                    {
                        startStaminaRechargeCooldown = true;
                    }
                    if (startStaminaRechargeCooldown)
                    {
                        if (!cooldownFinished)
                        {
                            //Debug.Log("Starting stamina cooldown counter...");

                            staminaCooldown += Time.deltaTime;

                            if (staminaCooldown >= staminaCooldownLimit)
                            {
                                cooldownFinished = true;
                            }
                        }
                        else if (cooldownFinished)
                        {
                            //Debug.Log("Stamina is recharging!");

                            staminaCooldown = 0;

                            currentStamina += staminaRecharge * Time.deltaTime;
                        }
                    }
                    //fix stamina if it goes over the limit
                    if (currentStamina > maxStamina)
                    {
                        //Debug.Log("Stamina is fully recharged!");

                        currentStamina = maxStamina;

                        StaminaCooldownFinished();
                    }
                }

                //jumping
                if (Input.GetKey(KeyCode.Space)
                    && isGrounded
                    && !isJumping
                    && canJump
                    && currentStamina >= 5)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -5.2f * gravity);
                    controller.stepOffset = 0;
                    currentStamina -= 5;
                    isJumping = true;
                    isGrounded = false;
                }
                else if (isGrounded
                         && isJumping)
                {
                    controller.stepOffset = 0.3f;
                    isJumping = false;
                }

                //crouching
                if (Input.GetKeyDown(KeyCode.LeftControl)
                    && isGrounded
                    && canCrouch)
                {
                    isCrouching = !isCrouching;

                    if (isSprinting)
                    {
                        isSprinting = false;
                    }

                    if (isCrouching)
                    {
                        //Debug.Log("Player is crouching!");

                        isGrounded = true;
                        velocity.y = -2f;

                        currentSpeed = crouchSpeed;

                        controller.height = crouchHeight;

                        PlayerCamera.transform.localPosition = cameraCrouchHeight;

                        if (currentStamina < maxStamina)
                        {
                            currentStamina += staminaRecharge * Time.deltaTime;
                            //Debug.Log("Stamina is recharging!");
                        }

                        if (currentStamina > maxStamina)
                        {
                            currentStamina = maxStamina;
                        }
                    }
                    else if (!isCrouching)
                    {
                        //Debug.Log("Player is no longer crouching...");

                        currentSpeed = walkSpeed;

                        controller.height = originalHeight;

                        PlayerCamera.transform.localPosition = cameraFullHeight;
                    }
                }
            }
        }
    }

    private void StaminaCooldownFinished()
    {
        startStaminaRechargeCooldown = false;
        cooldownFinished = false;
        staminaCooldown = 0;
    }
}