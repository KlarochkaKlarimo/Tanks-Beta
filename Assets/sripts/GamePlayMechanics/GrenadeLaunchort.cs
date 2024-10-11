using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLaunchort : MonoBehaviour
{
    [SerializeField] private List<SmokeGrenadesScrip> _grenades;
    [SerializeField] private int _grenadesInShoot;
    public KeyCode launchKey = KeyCode.G;

    void Update()
    {
        if (Input.GetKeyDown(launchKey) && _grenades.Count > 0)
        {
            var i = 0;
            var grenadeToRemove = _grenades.Count > _grenadesInShoot ? _grenadesInShoot: _grenades.Count;

            foreach (var grenade in _grenades)
            {
                if (i < grenadeToRemove && _grenades.Count > 0)
                {
                    i++;
                    grenade.LaunchSmokeGrenade();
                    grenade.transform.SetParent(null);
                }
                else
                {
                    break;
                }
                
            }
                _grenades.RemoveRange(0, grenadeToRemove);
        }
    }
}
