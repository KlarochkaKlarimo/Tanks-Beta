using UnityEngine;

public class ExplosiveReactiveArmour : MonoBehaviour
{
    [SerializeField] private int _heatDamagePenetrationReduction;
    [SerializeField] private int _heatDamageModulReduction;
    [SerializeField] private int _kineticDamagePenetrationReduction;
    [SerializeField] private int _kineticDamageModulReduction;

    public int GetModulDamage(int type )
    {
        switch (type) 
        {
        case 0: return _kineticDamageModulReduction;
        case 1: return _heatDamageModulReduction;
        }
        return 0;
    }
    public int GetPenitrationDamage(int type)
    {
        switch (type)
        {
            case 0: return _kineticDamagePenetrationReduction;
            case 1: return _heatDamagePenetrationReduction;
        }
        return 0;
    }
}
