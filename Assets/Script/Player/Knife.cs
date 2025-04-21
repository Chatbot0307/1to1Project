using System.Collections;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public float speed = 100f;
    public int damage = 1;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.magnitude > 50f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && Player.playerInstance != null)
        {
            StartCoroutine(Player.playerInstance.TeleportEffect(collision.transform.position));
        }
    }
}