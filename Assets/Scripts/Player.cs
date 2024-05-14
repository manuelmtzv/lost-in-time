using System;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public float jumpForce = 6.5f;
    public float walkSpeed = 5.2f;
    public float runSpeed = 6.8f;
    public TimeFlowState timeFlowState;
    public PlayerState playerState;
    public SliderBar healthBar;
    public bool isGrounded;
    public Light2D playerLight;
    private bool isRunning;
    private bool runTrigger;
    private bool canDoubleJump;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInput playerInput;
    private Transform bodyTransform;
    private bool isFacingRight;
    private float horizontalMovement;
    private AudioSource movementSfxSource;
    private GameObject[] playerBodyParts;

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
        movementSfxSource = GetComponent<AudioSource>();
        playerBodyParts = GameObject.FindGameObjectsWithTag("PlayerBodyPart");

        movementSfxSource.volume = 0.25f;

        playerState.Reset();
        healthBar.SetSliderMax(playerState.maxHealth);
    }

    void Update()
    {
        if (playerState.health <= 0) return;

        horizontalMovement = playerInput.actions["Movement"].ReadValue<Vector2>().x;
        runTrigger = playerInput.actions["Run"].ReadValue<float>() == 1;

        Flip();

        animator.SetBool("isJumping", !isGrounded);

        playerLight.intensity = timeFlowState.slowMo ? 1f : 1.8f;
        playerLight.pointLightOuterRadius = timeFlowState.slowMo ? 5f : 10f;
    }

    private void FixedUpdate()
    {
        if (playerState.health <= 0) return;

        isRunning = runTrigger && isGrounded;
        float speedMovement = isRunning ? runSpeed : walkSpeed;

        if (timeFlowState.slowMo)
        {
            speedMovement *= 2f;
        }

        EmitMovementSFX();

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
        if (playerState.health <= 0) return;

        float newJumpForce = timeFlowState.slowMo ? jumpForce * 1.5f : jumpForce * 1.35f;

        if (callbackContext.performed && isGrounded)
        {
            AudioManager.Instance.PlaySFX(GlobalAssets.Instance.playerJumpSound, 0.2f);
            rb.velocity = new Vector2(rb.velocity.x, newJumpForce);
            isGrounded = false;
            canDoubleJump = true;

            animator.SetBool("isJumping", !isGrounded);
        }
        else if (callbackContext.performed && canDoubleJump)
        {
            AudioManager.Instance.PlaySFX(GlobalAssets.Instance.playerDoubleJumpSound, 0.2f);
            rb.velocity = new Vector2(rb.velocity.x, newJumpForce);
            canDoubleJump = false;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        AudioManager.Instance.PlaySFX(GlobalAssets.Instance.playerDamageSound, 0.4f);
        playerState.health -= damageAmount;
        healthBar.SetSliderValue(playerState.health);

        if (playerState.health <= 0)
        {
            playerState.health = 0;
            Kill();
        }
    }

    public void Kill()
    {
        AudioManager.Instance.PlaySFX(GlobalAssets.Instance.playerDeathSound, 0.4f);

        Destroy(rb);
        Destroy(animator);
        Destroy(GetComponent<Collider2D>());

        Vector2 deathForce = new(isFacingRight ? 4 : -4, 5);

        SetRigidbodyOnPlayerParts(deathForce);
    }

    public void Heal(int healAmount)
    {
        AudioManager.Instance.PlaySFX(GlobalAssets.Instance.powerUpSound, 0.4f);
        playerState.health = Mathf.Min(playerState.health + healAmount, playerState.maxHealth);
        healthBar.SetSliderValue(playerState.health);
    }

    public void EmitMovementSFX()
    {
        movementSfxSource.pitch = 0.75f;

        if (isRunning)
        {
            movementSfxSource.pitch = 0.505f;
            movementSfxSource.clip = GlobalAssets.Instance.playerRunSound;
        }
        else if (horizontalMovement != 0 && isGrounded)
        {
            movementSfxSource.clip = GlobalAssets.Instance.playerWalkSound;
        }
        else
        {
            movementSfxSource.clip = null;
        }

        if (movementSfxSource.clip != null && !movementSfxSource.isPlaying)
        {
            movementSfxSource.Play();
        }
        else if (movementSfxSource.clip == null)
        {
            movementSfxSource.Stop();
        }
    }

    public void SetRigidbodyOnPlayerParts(Vector2 velocity)
    {
        foreach (GameObject part in playerBodyParts)
        {
            Collider2D partCollider = part.GetComponent<Collider2D>();
            partCollider.isTrigger = false;

            Rigidbody2D childRb = part.AddComponent<Rigidbody2D>() as Rigidbody2D;

            if (childRb != null)
            {
                childRb.velocity = velocity;
            }

        }
    }
}
