using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyAIResolver
{
    public static IEnemyAI Resolve(EnemyAIType type)
    {
        switch (type)
        {
            case EnemyAIType.RangedAI:
                return new RangedAI();
            default:
                return new RangedAI();
        }
    }
}