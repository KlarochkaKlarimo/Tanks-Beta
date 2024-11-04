using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConquestGameModeController : MonoBehaviour
{
    private Dictionary<PlayerTeam, float> _teamsGlobalScore = new Dictionary<PlayerTeam, float>();

    [SerializeField] private PointCapture[] _points;

    [SerializeField] private int _scoreForWin;

    private bool _isWin;

    private void FixedUpdate()
    {
        if(_isWin)
        {
            return;
        }

        foreach(var point in _points)
        {
            if (point.GetPointCaptureTeam() == PlayerTeam.None)
            {
                continue;
            }

            if (!_teamsGlobalScore.ContainsKey(point.GetPointCaptureTeam()))
            {
                _teamsGlobalScore[point.GetPointCaptureTeam()] = 0;
            }

            _teamsGlobalScore[point.GetPointCaptureTeam()] += Time.deltaTime;
        }

        if (_teamsGlobalScore.Count == 0)
        {
            return;
        }

        foreach (var score in _teamsGlobalScore)
        {
            if (score.Value > _scoreForWin)
            {
                _isWin = true;
                Debug.Log(score.Key.ToString() + " wins");
            }
        }
    }
}
