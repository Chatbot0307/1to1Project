using System.Collections;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private float maxDistanceFromCamera = 30f;

    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector3 cameraPositionCached;
    private float maxDistanceSqr;
    public float damage = 1f;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.isKinematic = false;

        cameraPositionCached = Camera.main.transform.position;
        maxDistanceSqr = maxDistanceFromCamera * maxDistanceFromCamera;
    }

    private void Start()
    {
        rb.velocity = direction * speed;
    }

    private void FixedUpdate()
    {
        if ((transform.position - cameraPositionCached).sqrMagnitude > maxDistanceSqr)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && Player.playerInstance != null)
        {
            StartCoroutine(Player.playerInstance.TeleportEffect(collision.transform.position));
        }
    }
}