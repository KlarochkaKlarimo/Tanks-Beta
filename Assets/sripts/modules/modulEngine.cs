
public class modulEngine : ModulBase
{
    public override void ModulDamaged()
    {
        base.ModulDamaged();
        driveControl.Torque /=5;
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
        driveControl.enabled = false;
    }

}
    
