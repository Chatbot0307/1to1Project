using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : MonoBehaviour
{
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private Transform firePoint;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowKnife();
        }
    }

    private void ThrowKnife()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector2 direction = (mouseWorldPos - firePoint.position).normalized;

        GameObject knifeObj = Instantiate(knifePrefab, firePoint.position, Quaternion.identity);
        Knife knife = knifeObj.GetComponent<Knife>();
        knife.Initialize(direction);
    }
}