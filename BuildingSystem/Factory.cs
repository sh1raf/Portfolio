using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Building
{
    [SerializeField] private Transform productionPlace;
    [SerializeField] private int baseMaxGrapeCount, baseExitProductionCount, maxLevel;
    [SerializeField] private float baseWorkTime;
    private int _maxGrapeCount, _exitProductionCount, _currentProductionCount;
    private float _workTime;
    private int _currentLevel = 0;
    private int _grapeCount;
    private bool _readyForLoad = true;
    public bool ReadyForLoad => _readyForLoad;

    private void Awake() 
    {

        int buff = GetComponentInParent<Tile>().PlaceForFactory;
        baseExitProductionCount += buff;
        baseMaxGrapeCount -= buff * 2;
        baseWorkTime -= buff * 7; 

        LevelUp();
    }

    public int Load(int count)
    {
        if(_readyForLoad)
        {
            if(_grapeCount + count > _maxGrapeCount)
            {
                var surPlus = count - (_maxGrapeCount - _grapeCount);
                _grapeCount = _maxGrapeCount;

                StartCoroutine(Work(_workTime, _grapeCount, _exitProductionCount));
                Debug.Log($"Factory Loaded with {_grapeCount}");
                return surPlus;
            }
            else
            {
                _grapeCount += count;

                if(_grapeCount == _maxGrapeCount)
                    StartCoroutine(Work(_workTime, _grapeCount, _exitProductionCount));

                Debug.Log($"Factory Loaded with {_grapeCount}");

                return 0;
            }
        }
        else
            return count;
    }

    public override IEnumerator Collect()
    {
        if(_currentProductionCount > 0)
        {
            for(int i = 0; i < _currentProductionCount; i++)
            {
                yield return new WaitForSeconds(1);
            }

            GameObject instantiated = Instantiate(productInstance, productionPlace.position, Quaternion.identity).gameObject;
            instantiated.GetComponent<Juice>().Count = _currentProductionCount;

            _currentProductionCount = 0;
            _readyForLoad = true;
        }
    }

    public bool LevelUp()
    {
        if(_currentLevel + 1 <= maxLevel)
        {
            _currentLevel++;
            _exitProductionCount = baseExitProductionCount * _currentLevel - 3;
            _maxGrapeCount = baseMaxGrapeCount * _currentLevel - 5;
            _workTime = baseWorkTime * _currentLevel - 15f;

            return true;
        }
        else
            return false;
    }

    private IEnumerator Work(float time, int fruitCount, int productionCount)
    {
        _readyForLoad = false;
        
        _grapeCount -= fruitCount;
        Debug.Log("StartWork");

        yield return new WaitForSeconds(time);

        Debug.Log($"Factory End Working with {productionCount} production");
        _currentProductionCount = productionCount;
    }

}
