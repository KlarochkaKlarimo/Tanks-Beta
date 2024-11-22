using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ChobiAssets.PTM;

[CreateAssetMenu(fileName = "Tanks Settings", menuName = "ScriptableObjects/TanksSettings")]

public class TanksSettingsCollection : ScriptableObject //MENU
{
    public TanksSetting[] settings;
}

[Serializable]

//TODO ADD ANOTHER WEAPON AMMO TYPE

public class TanksSetting //REAL SETTINGS
{
    public string tankName;
    public BulletSettings[] bullets;
    public int maxAmmorack;
}
