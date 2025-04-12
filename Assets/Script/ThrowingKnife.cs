using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 direction = mousePos - firePoint.position;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        bullet.GetComponent<Bullet>().SetDirection(direction);
    }
}
