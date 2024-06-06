using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulCaptain : CharacterBase
{
    [SerializeField] private EnemyVision _enemyVision;

    public override void ModulDamaged()
    {
        base.ModulDamaged();
        //damage komander igrok
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
        if (isModelDamaged)
        {
            // igrok umiraet 
            return;
        }
    }
}
