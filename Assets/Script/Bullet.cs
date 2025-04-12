using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
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
}
