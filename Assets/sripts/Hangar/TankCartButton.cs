using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankCartButton : MonoBehaviour
{
    [SerializeField] private Text _tankName;
    private HangarMenu _menu;
    private TanksSetting _tankSettings;

    public void SetData(HangarMenu menu, TanksSetting tankSettings)
    {
        _menu = menu;
        _tankSettings = tankSettings;
        _tankName.text = tankSettings.tankName;
    }
    
    public void TankCartHandler()
    {
        _menu.FindTank(_tankSettings);
    }
}
