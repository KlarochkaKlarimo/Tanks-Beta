using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LWS : MonoBehaviour
{
    [SerializeField] private Image _imageWarningSector;
    [SerializeField] private float _timer;
    private float _time;
    private bool _isWarning;

    public void LaserWarning(GameObject sector)
    {
        sector.SetActive(true);
        _time = 0;
        _isWarning = true;
        _imageWarningSector.color = new Color(_imageWarningSector.color.r, _imageWarningSector.color.g, _imageWarningSector.color.b, 1);
        StopAllCoroutines();
    }

    private void Update()
    {
        if (_isWarning)
        {
            _time += Time.deltaTime;
            if (_time > _timer)
            {
                StartCoroutine(FadeImage());
            }
        }
    }

    private IEnumerator FadeImage()
    {
        while (_imageWarningSector.color.a > 0)
        {
            _imageWarningSector.color = new Color(_imageWarningSector.color.r, _imageWarningSector.color.g, _imageWarningSector.color.b, _imageWarningSector.color.a - 0.1f);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
