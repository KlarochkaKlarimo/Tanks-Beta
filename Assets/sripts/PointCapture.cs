using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PointCapture : MonoBehaviour
{
    private List<PlayerTeamControll> _playersInPointTrigger = new List<PlayerTeamControll>();

    private Dictionary<PlayerTeam, float> _teamsScore = new Dictionary<PlayerTeam, float>();

    [SerializeField] private int _pointsToCapturePoint;
    [SerializeField] private float _captureRate;
    [SerializeField] private PlayerTeam _pointCaptureTeam;
    [SerializeField] private PlayerTeam _leadingTeam;

    [SerializeField] private GameObject _nonFlag;
    [SerializeField] private GameObject _blueFlag;
    [SerializeField] private GameObject _redFlag;

    public PlayerTeam GetPointCaptureTeam()
    {
        return _pointCaptureTeam;
    }

    private void Update()
    {
        if (_leadingTeam != PlayerTeam.None)
        {
            if (!_teamsScore.ContainsKey(_leadingTeam))
            {
                _teamsScore[_leadingTeam] = 0;
            }

            var isPointFree = true;

            foreach (var team in _teamsScore)
            {
                if (team.Key != _leadingTeam && team.Value > 0)
                {
                    _teamsScore[team.Key] -= Time.deltaTime * _captureRate;
                    isPointFree = false;
                }
            }

            if (isPointFree && _pointCaptureTeam != _leadingTeam)
            {
                _teamsScore[_leadingTeam] += Time.deltaTime * _captureRate;                
            }
            CheckCapture();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        PlayerTeamControll ktoVoshel = other.GetComponent<PlayerTeamControll>();

        if(ktoVoshel != null)
        {
            if (!_playersInPointTrigger.Contains(ktoVoshel))
            {
                _playersInPointTrigger.Add(ktoVoshel);

                CheckTeamAdvantage();
            }        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerTeamControll ktoVishel = other.GetComponent<PlayerTeamControll>();

        if (ktoVishel != null)
        {
            if (_playersInPointTrigger.Contains(ktoVishel))
            {
                _playersInPointTrigger.Remove(ktoVishel);
                CheckTeamAdvantage();
            }
        }
    }

    private PlayerTeam CheckTeamAdvantage()
    {
        if(_playersInPointTrigger.Count == 0)
        {
            return PlayerTeam.None;
        }

        Dictionary<PlayerTeam, int> teamCounts = new Dictionary<PlayerTeam, int>();

        foreach(var player in _playersInPointTrigger)
        {
            if (!teamCounts.ContainsKey(player._playerTeam))
            {
                teamCounts[player._playerTeam] = 0;
            }
            teamCounts[player._playerTeam]++;
        }

        int maxCount = 0;
        bool isTie = false;

        foreach(var team in teamCounts)
        {
            if(team.Value > maxCount)
            {
                _leadingTeam = team.Key;
                maxCount = team.Value;
                isTie = false;
            }

            else if (team.Value == maxCount)
            {
                isTie = true;
            }
        }

        if (isTie)
        {
            _leadingTeam = PlayerTeam.None;
            return _leadingTeam;
        }

        return _leadingTeam;
    }

    private void CheckCapture()
    {
        if (_teamsScore[_leadingTeam] >= _pointsToCapturePoint)
        {
            _pointCaptureTeam = _leadingTeam;
            _nonFlag.SetActive(false);
            _redFlag.SetActive(_pointCaptureTeam == PlayerTeam.Red);
            _blueFlag.SetActive(_pointCaptureTeam == PlayerTeam.Blue);
        }

        else
        {
            _pointCaptureTeam = PlayerTeam.None;
            _nonFlag.SetActive(true);
            _redFlag.SetActive(false);
            _blueFlag.SetActive(false);
        }
    }
}
