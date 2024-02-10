using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LWSSector : MonoBehaviour
{
    [SerializeField] private LWS _lws;
    [SerializeField] private GameObject _warningSector;
    

    public void LaserWarning()
    {
        _lws.LaserWarning(_warningSector);
    }

}
