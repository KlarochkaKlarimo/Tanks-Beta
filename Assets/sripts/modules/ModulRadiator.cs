using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulRadiator : ModulBase
{
    public override void ModulDamaged()
    {
        base.ModulDamaged();
        //TO DO nagrev dvigatelya i on nachinaet poluchat damage
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
        //TO DO nagrev dvigatelya i on nachinaet poluchat damage no bistree
    }
}
