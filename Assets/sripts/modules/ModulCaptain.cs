using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulCaptain : CharacterBase
{
    public override void ModulDamaged()
    {
        base.ModulDamaged();
        // TODO damage komander igrok
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
        if (isModelDamaged)
        {
            // TODO igrok umiraet 
            return;
        }
    }
}
