
public class TransimitionModul : ModulBase
{
    public override void ModulDamaged()
    {
        base.ModulDamaged();
        //tankWheelControl.steerTorque /=5;
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
       // tankWheelControl.enabled = false;
    }
    private void Awake()
    {      
        driveControl.turnDivider = 0;
    }
}
