using UnityEngine;
using ChobiAssets.PTM;
public class ExplosiveReactiveArmour : MonoBehaviour
{
    [SerializeField] private int _heatDamagePenetrationReduction;
    [SerializeField] private int _heatDamageModulReduction;
    [SerializeField] private int _kineticDamagePenetrationReduction;
    [SerializeField] private int _kineticDamageModulReduction;

    public int GetModulDamage(BulletType type)
    {
        switch (type) 
        {
        case BulletType.APFSDS: return _kineticDamageModulReduction;
        default: return _heatDamageModulReduction;
        }       
    }
    public int GetPenitrationDamage(BulletType type)
    {
        switch (type)
        {
            case BulletType.APFSDS: return _kineticDamagePenetrationReduction;
            default: return _heatDamagePenetrationReduction;
        }
    }
}
