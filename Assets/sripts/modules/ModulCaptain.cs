using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulCaptain : CharacterBase
{
    [SerializeField] private EnemyVision _enemyVision;

    public override void ModulDamaged()
    {
        base.ModulDamaged();
        //_enemyVision.distance /=2;
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
        if (isModelDamaged)
        {
            //_enemyVision.distance /= 2;
            return;
        }
        //_enemyVision.distance /= 2;
    }

    
}
