using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player playerInstance;

    public TrailRenderer trail;
    public GameObject particleSystem;

    private float horizontal;
    private bool isFacingRight = true;
    private bool isMoving;
    private bool canJumpAfterTeleport = false;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private float speed = 12f;
    [SerializeField] private float jumpingPower = 20f;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private void Awake()
    {
        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Jump();

        WallSlide();
        WallJump();

        if(!isWallJumping)
        {
            Flip();
        }
        
        Particle();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if(!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if ((Input.GetButtonDown("Jump") && IsGrounded()) || (Input.GetButtonDown("Jump") && canJumpAfterTeleport))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if(IsWalled() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Particle()
    {
        if (IsGrounded() == false)
        {
            particleSystem.SetActive(false);
        }
        else if (Input.GetButton("Horizontal"))
        {
            particleSystem.SetActive(true);
        }
        else if (Input.GetButtonUp("Horizontal"))
        {
            particleSystem.SetActive(false);
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public IEnumerator TeleportEffect(Vector3 enemyPos)
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (trail != null)
        {
            trail.emitting = true;

            yield return new WaitForSecondsRealtime(0.2f);

            Vector3 teleportPos;

            if (transform.position.x < enemyPos.x)
            {
                if (horizontal == 1)
                    teleportPos = enemyPos + Vector3.right;
                else
                    teleportPos = enemyPos + Vector3.left * 0.5f;
            }
            else
            {
                if (horizontal == -1)
                    teleportPos = enemyPos + Vector3.left;
                else
                    teleportPos = enemyPos + Vector3.right * 0.5f;
            }

            transform.position = teleportPos;

            trail.emitting = false;

            StartCoroutine(CameraShake.Instance.Shake(0.2f, 0.2f));
        }
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        canJumpAfterTeleport = true;
        yield return new WaitForSecondsRealtime(0.3f);
        canJumpAfterTeleport = false;
    }
}
