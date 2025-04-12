using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int moveSpeed = 10;

    public int jumpCount = 1;
    public int jumpForce = 5;

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
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
}
