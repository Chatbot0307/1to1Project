using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed;

    private bool isShaking = false;

    private void Update()
    {
        if (!isShaking && target != null)
        {
            Vector3 targetPos = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
            transform.position = new Vector3(targetPos.x, targetPos.y, -10);
        }
    }

    public void SetShakeState(bool state)
    {
        isShaking = state;
    }
}