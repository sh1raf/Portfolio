using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GrapeTree : Building
{
    [SerializeField] private Transform collectPlace;
    [SerializeField] private float baseTimeToGrowUp;
    [SerializeField] private float timeOfRotting;
    [SerializeField] private int baseExitGrapeCount;
    [SerializeField] private int maxLevel;
    private int _currentLevel;
    private int _grapeCount, _exitGrapeCount;
    private float _timeToGrowUp;
    private int _growingUpCount;

    private void Awake() 
    {
        _currentLevel = 0;

        int buff = GetComponentInParent<Tile>().PlaceForTree;
        baseExitGrapeCount += buff;
        baseTimeToGrowUp -= buff * 10f;

        StartCoroutine(GrowingUp(baseTimeToGrowUp, 0));
    }

    public override IEnumerator Collect()
    {
        if(_exitGrapeCount != 0)
        {
            _growingUpCount++;

            Debug.Log("StartCollecting");
            for(int i = 0; i < Mathf.CeilToInt(_exitGrapeCount / 2); i++)
            {
                yield return new WaitForSeconds(1);
            }
            var instantiated = Instantiate(productInstance, collectPlace.position, Quaternion.identity);
            instantiated.Count = _exitGrapeCount;

            _exitGrapeCount = 0;
            Debug.Log("EndCollecting");

            StartCoroutine(GrowingUp(_timeToGrowUp, _grapeCount));
        }
    }

    private IEnumerator GrowingUp(float time, int count)
    {
        _growingUpCount++;

        yield return new WaitForSeconds(time);

        if(_currentLevel + 1 <= maxLevel)
        {
            _exitGrapeCount = count;

            _currentLevel++;
            if(_currentLevel > 1)
            {
                _grapeCount = baseExitGrapeCount * _currentLevel - 3;
                _timeToGrowUp = (baseTimeToGrowUp * _currentLevel) - 15f;

                StartCoroutine(Rotting(_growingUpCount));
            }
            else
            {
                _grapeCount = baseExitGrapeCount;
                _timeToGrowUp = baseTimeToGrowUp;

                StartCoroutine(GrowingUp(_timeToGrowUp, _grapeCount));
            }
            Debug.Log("GrowUp Completed, Current level: " + _currentLevel);
        }
        else
        {
            _exitGrapeCount = count;
            Debug.Log("GrowUp Completed, Current level: " + _currentLevel);
            StartCoroutine(Rotting(_growingUpCount));
        }
    }

    private IEnumerator Rotting(int count)
    {
        Debug.Log("StartRotting");
        yield return new WaitForSeconds(timeOfRotting);
        Debug.Log("EndRotting");

        if(count == _growingUpCount)
        {
            Debug.Log("Grape was rotted");
            _exitGrapeCount = 0;
            StartCoroutine(GrowingUp(_timeToGrowUp, _grapeCount));
        }
        else
        {
            Debug.Log("WTH");
        }
    }
}
