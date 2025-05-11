using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [SerializeField] ParticleSystem attackParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Knife"))
        {
            attackParticle.Play();
        }
    }
}
