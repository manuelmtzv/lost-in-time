using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor.Callbacks;
using System;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public float jumpForce = 8f;
    public float walkSpeed = 3f;
    public float runSpeed = 4f;
    public TimeFlowState timeFlowState;
    public PlayerState playerState;
    public SliderBar healthBar;
    public bool isGrounded;
    private bool isRunning;
    private bool runTrigger;
    private bool canDoubleJump;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInput playerInput;
    private Transform bodyTransform;
    private bool isFacingRight;
    private float horizontalMovement;

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
            canDoubleJump = true;

            animator.SetBool("isJumping", !isGrounded);
        }
        else if (callbackContext.performed && canDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.5f);
            canDoubleJump = false;
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

    public void Heal(int healAmount)
    {
        playerState.health = Mathf.Min(playerState.health + healAmount, playerState.maxHealth);
        healthBar.SetSliderValue(playerState.health);
    }
}
