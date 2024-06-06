
public class ModulGunner : CharacterBase
{
    public override void ModulDamaged()
    {
        base.ModulDamaged();
        aimingControl.Turret_Speed_Multiplier /=2;
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
        aimingControl.Turret_Speed_Multiplier = 0;
    }
}
