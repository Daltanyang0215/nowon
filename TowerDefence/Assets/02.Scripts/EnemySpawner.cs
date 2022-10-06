using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    private List<StageInfo> _stageList = new List<StageInfo>();
    private List<float[]> _timersList = new List<float[]>();
    private List<float[]> _delayTimersList = new List<float[]>();
    private List<int[]> _spawnCountersList = new List<int[]>();
    private List<List<GameObject>> _enemiesSpawndList = new List<List<GameObject>>();
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform[] _goalPoints;

    [SerializeField] private SkipButtonUI _skipButtonUIPrefab;
    private SkipButtonUI[] _skipButtonsBuffer = new SkipButtonUI[10];

    public event Action<int> OnStageFinished;
    private bool _spawnFinishedTrigger;
    private bool spawnFinishedTrigger
    {
        set
        {
            if(value && _spawnFinishedTrigger == false)
            {
                PopUpSkipButtons();
            }
            _spawnFinishedTrigger = value;
        }
        get
        {
            return _spawnFinishedTrigger;
        }
    }

    public void DestroyAllSkipButtons()
    {
        for (int i = 0; i < _skipButtonsBuffer.Length; i++)
        {
            if (_skipButtonsBuffer[i] != null)
            {
                Destroy(_skipButtonsBuffer[i].gameObject);
            }
        }
    }

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
    private void Start()
    {
        LevelInfo levelinfo = GamePlay.instance.levelInfo;
        foreach (StageInfo stageInfo in levelinfo.stagesInfo)
        {
            foreach (EnemySpawnData enemySpawnData in stageInfo.enemySpawnDataList)
            {
                ObjectPool.Instance.AddPoolElement(enemySpawnData.poolElement);
            }
        }

        ObjectPool.Instance.InstantiateAllPoolElement();
    }

    private void Update()
    {
        for (int i = _stageList.Count - 1; i >= 0; i--)
        {
            bool tmpSpawnFinished = true;
            for (int j = 0; j < _stageList[i].enemySpawnDataList.Count; j++)
            {
                if (_spawnCountersList[i][j] > 0)
                {
                            tmpSpawnFinished = false;
                    if (_delayTimersList[i][j] < 0)
                    {
                        if (_timersList[i][j] < 0)
                        {
                            //GameObject go = Instantiate(_stageList[i].enemySpawnDataList[j].poolElement.prefab, _spawnPoints[_stageList[i].enemySpawnDataList[j].spawnPointIndex].position, Quaternion.identity);
                            GameObject go = ObjectPool.Instance.Spawn(_stageList[i].enemySpawnDataList[j].poolElement.name, _spawnPoints[_stageList[i].enemySpawnDataList[j].spawnPointIndex].position);
                            _enemiesSpawndList[i].Add(go);

                            int tmpId = _stageList[i].id;
                            go.GetComponent<Enemy>().DieEventClear();
                            go.GetComponent<Enemy>().OnDie += () =>
                            {
                                int tmpIdx = _stageList.FindIndex(stageInfo => stageInfo.id == tmpId);
                                if (tmpIdx >= 0)
                                {
                                    _enemiesSpawndList[tmpIdx].Remove(go);
                                    if (_enemiesSpawndList[tmpIdx].Count == 0)
                                    {
                                        OnStageFinished(tmpId);
                                        _stageList.RemoveAt(tmpIdx);
                                        _timersList.RemoveAt(tmpIdx);
                                        _delayTimersList.RemoveAt(tmpIdx);
                                        _spawnCountersList.RemoveAt(tmpIdx);
                                    }
                                }
                            };
                            go.GetComponent<EnemyMove>().SetStartEnd(
                                _spawnPoints[_stageList[i].enemySpawnDataList[j].spawnPointIndex],
                                _goalPoints[_stageList[i].enemySpawnDataList[j].goalPointIndex]);
                            _timersList[i][j] = _stageList[i].enemySpawnDataList[j].term;
                            _spawnCountersList[i][j]--;
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

            if (_stageList[i].id == GamePlay.instance.currentStageId)
            {
                spawnFinishedTrigger = tmpSpawnFinished;
            }
        }
    }

    private void PopUpSkipButtons()
    {
        if (GamePlay.instance.currentStage >= GamePlay.instance.levelInfo.stagesInfo.Count - 1) return;

        HashSet<int> spawnPointIndexSet = new HashSet<int>();
        foreach (var enemySpawnData in GamePlay.instance.levelInfo.stagesInfo[GamePlay.instance.currentStage + 1].enemySpawnDataList)
        {
            spawnPointIndexSet.Add(enemySpawnData.spawnPointIndex);
        }

        foreach (var index in spawnPointIndexSet)
        {
            _skipButtonsBuffer[index] = Instantiate(_skipButtonUIPrefab, _spawnPoints[index].position + Vector3.up, _skipButtonUIPrefab.transform.rotation);
            _skipButtonsBuffer[index].AddButtonOnClickListener(() =>
            {
                GamePlay.instance.NextStage();
                DestroyAllSkipButtons();
            });

        }

    }
}
