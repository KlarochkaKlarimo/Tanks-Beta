using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Teams
{
    None,
    Blue,
    Red
}

public class PointCapture : MonoBehaviour
{
    [SerializeField] private int _pointsToCapturePoint;
    [SerializeField] private float _captureRate;
    [SerializeField] private int _currentBluePoints;
    [SerializeField] private int _currentRedPoints;
    [SerializeField] private Teams _pointCaptureTeam;

    [SerializeField] private GameObject _nonFlag;
    [SerializeField] private GameObject _blueFlag;
    [SerializeField] private GameObject _redFlag;

    private int _blueLayer;
    private int _redLayer;

    private bool _isBlueCapturing = false;
    private bool _isRedCapturing = false;
    private bool _isInTrigger = false;

    public Teams GetPointCaptureTeam()
    {
        return _pointCaptureTeam;
    }

    private void Start()
    {
        _blueLayer = LayerMask.NameToLayer("Ally");
        _redLayer = LayerMask.NameToLayer("Enemy");
    }

    private void OnTriggerEnter(Collider other)
    {
        int ktoVoshel = other.gameObject.layer;

        if (ktoVoshel == _blueLayer)
        {
            _isInTrigger = true;
            _isBlueCapturing = true;
            _isRedCapturing = false;
            StartCoroutine(PointCaptureCoruetine());
        }

        else if (ktoVoshel == _redLayer)
        {
            _isInTrigger = true;
            _isRedCapturing = true;
            _isBlueCapturing = false;
            StartCoroutine(PointCaptureCoruetine());           
        }

    }

    private void OnTriggerExit(Collider other)
    {
        int ktoVishel = other.gameObject.layer;

        if (ktoVishel == _blueLayer || ktoVishel == _redLayer)
        {
            _isInTrigger = false;
        }
    }

    private IEnumerator PointCaptureCoruetine()
    {
        while (_isInTrigger)
        {
            if (_isBlueCapturing)
            {
                if (_currentRedPoints > 0)
                {
                    _currentRedPoints--;
                }
                else if (_currentBluePoints < _pointsToCapturePoint)
                {
                    _currentBluePoints++;
                    CheckCapture();
                }
            }

            else if (_isRedCapturing)
            {
                if (_currentBluePoints > 0)
                {
                    _currentBluePoints--;
                }

                else if (_currentRedPoints < _pointsToCapturePoint)
                {
                    _currentRedPoints++;
                    CheckCapture();
                }
            }

            else
            {
                CheckCapture();
                break;
            }

            yield return new WaitForSeconds(_captureRate);
        }
    }

    private void CheckCapture()
    {
        if (_currentBluePoints >= _pointsToCapturePoint)
        {
            _pointCaptureTeam = Teams.Blue;
            _nonFlag.SetActive(false);
            _redFlag.SetActive(false);
            _blueFlag.SetActive(true);
            Debug.Log("Pont is captured by blue");
        }

        if (_currentRedPoints >= _pointsToCapturePoint)
        {
            _pointCaptureTeam = Teams.Red;
            _nonFlag.SetActive(false);
            _blueFlag.SetActive(false);
            _redFlag.SetActive(true);
            Debug.Log("Pont is captured by red");
        }
    }
}
