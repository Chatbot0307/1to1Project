using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player playerInstance;

    [Header("Stats")]
    public int moveSpeed = 10;

    public int jumpCount = 1;
    public int jumpForce = 5;

    [Header("Effects")]
    public TrailRenderer trail;

    private Rigidbody2D rigid;

    private void Awake()
    {
        if(playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Jump();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");

        transform.position += new Vector3(h, 0, 0) * moveSpeed * Time.deltaTime;
    }

    private void Jump()
    {
        if (jumpCount != 0 && Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            jumpCount--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 1;
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

            transform.position = enemyPos;

            trail.emitting = false;

            StartCoroutine(CameraShake.Instance.Shake(0.2f, 0.2f));
        }
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
