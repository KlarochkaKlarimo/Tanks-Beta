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
        if (Input.GetKeyDown(launchKey))
        {
            if (_grenades.Count < _grenadesInShoot)
            {
                return;
            }
            var i = 0;

            foreach (var grenade in _grenades)
            {
                if (i < _grenadesInShoot)
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
             _grenades.RemoveRange(0, _grenadesInShoot);
        }
    }
}
