
public class ModulCannon : ModulBase
{
    
    public override void ModulDamaged()
    {
        base.ModulDamaged();
        cannonFire.CannonDamage();
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
        cannonFire.enabled = false;
    }
}
