using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Enemy Stats")]
    public string enemyName;
    public GameObject prefab;
    public float maxHealth;
    public float moveSpeed;
    public float damage;

    [Header("Enemy Type")]
    public EnemyAIType aiType;
}
