using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulCaptain : CharacterBase
{
    [SerializeField] private EnemyVision _enemyVision;

    public override void modulDamaged()
    {
        base.modulDamaged();
        _enemyVision.distance /=2;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        if (isModelDamaged)
        {
            _enemyVision.distance /= 2;
            return;
        }
        _enemyVision.distance /= 2;
    }

    
}
