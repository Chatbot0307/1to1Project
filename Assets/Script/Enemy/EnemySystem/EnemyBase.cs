using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyDataSO data;
    private float currentHealth;
    private IEnemyAI aiBehavior;

    void Start()
    {
        currentHealth = data.maxHealth;
        aiBehavior = EnemyAIResolver.Resolve(data.aiType);
    }

    void Update()
    {
        aiBehavior?.RunAI(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Knife"))
        {
            Knife knifeDmg = collision.GetComponent<Knife>();
            TakeDamage(knifeDmg.damage);
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log($"{data.enemyName} died.");
        Destroy(gameObject);
    }
}