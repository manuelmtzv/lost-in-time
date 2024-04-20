using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor.Callbacks;
using System;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [SerializeField] float jumpForce = 8f;
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] TimeFlowState timeFlowState;
    [SerializeField] PlayerState playerState;
    [SerializeField] SliderBar healthBar;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInput playerInput;
    private Transform bodyTransform;
    private bool isFacingRight;
    private float horizontalMovement;
    public bool isGrounded;
    private bool isRunning;
    private bool runTrigger;

    public bool IsFacingRight
    {
        get => isFacingRight;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Animator>();
        bodyTransform = GameObject.FindGameObjectWithTag("PlayerBody").transform;

        playerState.health = playerState.maxHealth;
        healthBar.SetSliderMax(playerState.maxHealth);
    }

    void Update()
    {
        horizontalMovement = playerInput.actions["Movement"].ReadValue<Vector2>().x;
        runTrigger = playerInput.actions["Run"].ReadValue<float>() == 1;

        Flip();

        animator.SetBool("isJumping", !isGrounded);
    }

    private void FixedUpdate()
    {
        isRunning = runTrigger && isGrounded;
        float speedMovement = isRunning ? runSpeed : walkSpeed;

        if (timeFlowState.slowMo)
        {
            speedMovement *= 2f;
        }

        rb.velocity = new Vector2(horizontalMovement * speedMovement, rb.velocity.y);

        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("isRunning", Convert.ToInt32(isRunning));
    }

    private void CheckFacingDirection()
    {
        isFacingRight = bodyTransform.localScale.x > 0;
    }

    private void Flip()
    {
        CheckFacingDirection();

        if ((isFacingRight && horizontalMovement < 0) || (!isFacingRight && horizontalMovement > 0))
        {
            isFacingRight = !isFacingRight;
            bodyTransform.localScale = new Vector3(bodyTransform.localScale.x * -1f, bodyTransform.localScale.y, bodyTransform.localScale.z);
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;

            animator.SetBool("isJumping", !isGrounded);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        playerState.health -= damageAmount;
        healthBar.SetSliderValue(playerState.health);

        if (playerState.health <= 0)
        {
            playerState.health = 0;
        }

        Debug.Log("Player health: " + playerState.health);

        if (playerState.IsDead)
        {
            Debug.Log("Player is dead");
        }
    }
}
