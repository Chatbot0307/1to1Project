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
    [SerializeField] private float speed = 12f;
    [SerializeField] private float jumpingPower = 20f;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

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
        Flip();
        Particle();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpingPower, ForceMode2D.Impulse);
        }
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

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
    }
}
