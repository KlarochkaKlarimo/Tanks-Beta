using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ChobiAssets.PTM;

[CreateAssetMenu(fileName = "Tanks Settings", menuName = "ScriptableObjects/TanksSettings")]

public class TanksSettings : ScriptableObject //MENU
{
    public TanksSetting[] settings;
}

[Serializable]

public class TanksSetting //REAL SETTINGS
{
    public string tankName;
    public BulletSettings[] bullets;
    public int maxAmmorack;
}
