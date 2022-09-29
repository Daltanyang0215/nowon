using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<StageInfo> _stageList = new List<StageInfo>();
    private List<float[]> _timersList = new List<float[]>();
    private List<float[]> _delayTimersList = new List<float[]>();
    private List<int[]> _spawnCountersList = new List<int[]>();
    private List<List<GameObject>> _enemiesSpawndList = new List<List<GameObject>>();
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform[] _goalPoints;

    public event Action<int> OnStageFinished;

    public void StartSpawn(StageInfo stageInfo)
    {
        _stageList.Add(stageInfo);

        float[] tmpTimerList = new float[stageInfo.enemySpawnDataList.Count];
        float[] delayTimerList = new float[stageInfo.enemySpawnDataList.Count];
        int[] tmpSpawnCountersList = new int[stageInfo.enemySpawnDataList.Count];

        for (int i = 0; i < stageInfo.enemySpawnDataList.Count; i++)
        {
            tmpTimerList[i] = stageInfo.enemySpawnDataList[i].term;
            delayTimerList[i] = stageInfo.enemySpawnDataList[i].delay;
            tmpSpawnCountersList[i] = stageInfo.enemySpawnDataList[i].poolElement.num;
        }

        _timersList.Add(tmpTimerList);
        _delayTimersList.Add(delayTimerList);
        _spawnCountersList.Add(tmpSpawnCountersList);
        _enemiesSpawndList.Add(new List<GameObject>());
    }

    private void Update()
    {
        for (int i = _stageList.Count - 1; i >= 0; i--)
        {
            bool isSpawnFinished = true;
            for (int j = 0; j < _stageList[i].enemySpawnDataList.Count; j++)
            {
                if (_delayTimersList[i][j] < 0)
                {
                    if (_timersList[i][j] < 0)
                    {
                        if (_spawnCountersList[i][j] > 0)
                        {
                            GameObject go = Instantiate(_stageList[i].enemySpawnDataList[j].poolElement.prefab, _spawnPoints[i].position, Quaternion.identity);
                            _enemiesSpawndList[i].Add(go);

                            int tmpId = _stageList[i].id;
                            go.GetComponent<Enemy>().OnDie += () =>
                            {
                                int tmpIdx = _stageList.FindIndex(stageInfo => stageInfo.id == tmpId);

                                _enemiesSpawndList[tmpIdx].Remove(go);
                                if (_enemiesSpawndList[tmpIdx].Count == 0)
                                {
                                    OnStageFinished(tmpId);
                                    _stageList.RemoveAt(tmpIdx);
                                    _timersList.RemoveAt(tmpIdx);
                                    _delayTimersList.RemoveAt(tmpIdx);
                                    _spawnCountersList.RemoveAt(tmpIdx);
                                }
                            };
                            go.GetComponent<EnemyMove>().SetStartEnd(
                                _spawnPoints[_stageList[i].enemySpawnDataList[j].spawnPointIndex],
                                _goalPoints[_stageList[i].enemySpawnDataList[j].goalPointIndex]);
                            _timersList[i][j] = _stageList[i].enemySpawnDataList[j].term;
                            _spawnCountersList[i][j]--;
                            isSpawnFinished = false;
                        }
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

            if (isSpawnFinished)
            {

            }
        }
    }
}
