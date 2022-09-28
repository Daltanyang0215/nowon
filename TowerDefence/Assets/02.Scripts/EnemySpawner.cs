using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<StageInfo> _stageList = new List<StageInfo>();
    private List<float[]> _timersList = new List<float[]>();
    private List<float[]> _delayTimersList = new List<float[]>();
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform[] _goalPoints;

    public void StartSpawn(StageInfo stageInfo)
    {
        _stageList.Add(stageInfo);

        float[] tmpTimerList = new float[stageInfo.enemySpawnDataList.Count];
        float[] delayTimerList = new float[stageInfo.enemySpawnDataList.Count];

        for (int i = 0; i < tmpTimerList.Length; i++)
        {
            tmpTimerList[i] = stageInfo.enemySpawnDataList[i].term;
        }
        for (int i = 0; i < delayTimerList.Length; i++)
        {
            delayTimerList[i] = stageInfo.enemySpawnDataList[i].delay;
        }

        _timersList.Add(tmpTimerList);
        _delayTimersList.Add(delayTimerList);
    }

    private void Update()
    {
        for (int i = 0; i < _stageList.Count; i++)
        {
            for (int j = 0; j < _stageList[i].enemySpawnDataList.Count; j++)
            {
                if (_delayTimersList[i][j] >= 0)
                {
                    if (_timersList[i][j] <= 0)
                    {
                        GameObject go = Instantiate(_stageList[i].enemySpawnDataList[j].poolElement.prefab, _spawnPoints[i].position, Quaternion.identity);
                        go.GetComponent<EnemyMove>().SetStartEnd(
                            _spawnPoints[_stageList[i].enemySpawnDataList[j].spawnPointIndex],
                            _goalPoints[_stageList[i].enemySpawnDataList[j].goalPointIndex]);
                        _timersList[i][j] = _stageList[i].enemySpawnDataList[j].term;
                    }
                    else
                    {
                        _timersList[i][j] -= Time.deltaTime;
                    }
                }
                else
                {
                    _delayTimersList[i][j] -= Time.deltaTime;
                }
            }

        }
    }
}
