using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConquestGameModeController : MonoBehaviour
{
    [SerializeField] private PointCapture[] _points;

    [SerializeField] private int _scoreForWin;
    [SerializeField] private float _blueScore;
    [SerializeField] private float _redScore;

    private bool _isWin;

    private void FixedUpdate()
    {
        if(_isWin)
        {
            return;
        }

        foreach(var point in _points)
        {
            switch (point.GetPointCaptureTeam())
            {
                case Teams.Blue:
                    _blueScore += Time.fixedDeltaTime;
                    break;

                case Teams.Red:
                    _redScore += Time.fixedDeltaTime;
                    break;
            }
        }

        if (_blueScore > _scoreForWin)
        {
            Debug.Log("Blue wins");
            _isWin = true;
        }

        else if (_redScore > _scoreForWin)
        {
            Debug.Log("Red wins");
            _isWin = true;
        }
    }
}
