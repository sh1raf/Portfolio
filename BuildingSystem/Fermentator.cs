using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fermentator : Building
{
    [SerializeField] private Transform placeForWine;
    [SerializeField] private int maxJuiceBoxCount;
    [SerializeField] private float timeBeforeStart;
    [SerializeField] private float timeToFermentation;

    private int _currentJuiceBoxCount;

    private int _wineBottleCount;
    private bool _fermentationStart;
    private bool _readyCollect = false;
    public bool ReadyCollect => _readyCollect;

    public int Load(int count)
    {
        if(_fermentationStart == false)
        {
            if(_currentJuiceBoxCount + count > maxJuiceBoxCount)
            {
                var surPlus = count - (maxJuiceBoxCount - _currentJuiceBoxCount);
                _currentJuiceBoxCount = maxJuiceBoxCount;

                StartCoroutine(Waiting());
                return surPlus;
            }
            else
            {
                _currentJuiceBoxCount += count;
                StartCoroutine(Waiting());

                return 0;
            }
        }
        else
            return count;
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(timeBeforeStart);

        if(_fermentationStart == false)
            StartCoroutine(Fermentation());
    }

    private IEnumerator Fermentation()
    {
        _readyCollect = false;
        _fermentationStart = true;

        yield return new WaitForSeconds(timeToFermentation);

        _wineBottleCount = _currentJuiceBoxCount;

        _fermentationStart = false;
        _readyCollect = true;
    }

    public override IEnumerator Collect()
    {
        if(_wineBottleCount != 0)
        {
            for(int i = 0; i < _wineBottleCount; i++)
            {
                yield return new WaitForSeconds(1);
            }
            Instantiate(productInstance, placeForWine.position, Quaternion.identity).Count = _wineBottleCount;
            
            _wineBottleCount = 0;
            _currentJuiceBoxCount = 0;
            _readyCollect = false;
        }
    }
}
