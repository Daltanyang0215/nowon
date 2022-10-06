using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;
    public static ObjectPool Instance
    {
        get
        {
            if (_instance == null)
                _instance = Instantiate(Resources.Load<ObjectPool>("Assets/ObjectPool"));
            return _instance;
        }
    }

    private List<PoolElement> _poolElements = new List<PoolElement>();
    private List<GameObject> _spawnedObjects = new List<GameObject>();
    private Dictionary<string, Queue<GameObject>> spawnedQueuePaires = new Dictionary<string, Queue<GameObject>>();

    public void AddPoolElement(PoolElement poolElement) => _poolElements.Add(poolElement);

    public void InstantiateAllPoolElement()
    {
        foreach (PoolElement poolElement in _poolElements)
        {
            if (spawnedQueuePaires.ContainsKey(poolElement.name) == false)
                spawnedQueuePaires.Add(poolElement.name, new Queue<GameObject>());


            for (int i = 0; i < poolElement.num; i++)
            {
                InstantiatePoolElement(poolElement);
            }
        }
    }

    public GameObject Spawn(string name, Vector3 spawnPoint)
    {
        if (spawnedQueuePaires.ContainsKey(name) == false)
            return null;

        if (spawnedQueuePaires[name].Count == 0)
        {
            PoolElement poolElement = _poolElements.Find(element => element.name == name);
            if (poolElement != null)
            {
                for (int i = 0; i < Math.Ceiling(Mathf.Log10(poolElement.num)); i++)
                {
                    InstantiatePoolElement(poolElement);

                }
            }
        }

        GameObject go = spawnedQueuePaires[name].Dequeue();
        go.transform.position = spawnPoint;
        go.SetActive(true);
        go.transform.SetParent(null);
        return go;
    }
    public GameObject Spawn(string name, Vector3 spawnPoint, Quaternion rotation)
    {
        if (spawnedQueuePaires.ContainsKey(name) == false)
            return null;

        if (spawnedQueuePaires[name].Count == 0)
        {
            PoolElement poolElement = _poolElements.Find(element => element.name == name);
            if (poolElement != null)
            {
                for (int i = 0; i < Math.Ceiling(Mathf.Log10(poolElement.num)); i++)
                {
                    InstantiatePoolElement(poolElement);

                }
            }
        }

        GameObject go = spawnedQueuePaires[name].Dequeue();
        go.transform.position = spawnPoint;
        go.transform.rotation = rotation;
        go.SetActive(true);
        go.transform.SetParent(null);
        return go;
    }
    public void Return(GameObject obj)
    {
        if (spawnedQueuePaires.ContainsKey(obj.name) == false)
        {
            return;
        }

        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        spawnedQueuePaires[obj.name].Enqueue(obj);
        RearrangeSiblings(obj);
        obj.SetActive(false);
    }

    public void Return(GameObject obj, float sec)
    {
        if (spawnedQueuePaires.ContainsKey(obj.name) == false)
        {
            return;
        }

        StartCoroutine(E_Return(obj, sec));
    }

    private void Awake()
    {
        transform.position = new Vector3(5000, 5000, 5000);
    }

    IEnumerator E_Return(GameObject obj, float sec)
    {
        yield return new WaitForSeconds(sec);

        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        spawnedQueuePaires[obj.name].Enqueue(obj);
        RearrangeSiblings(obj);
        obj.SetActive(false);
    }

    private GameObject InstantiatePoolElement(PoolElement poolElement)
    {
        GameObject go = Instantiate(poolElement.prefab, transform);
        go.name = poolElement.name;
        go.SetActive(false);
        spawnedQueuePaires[poolElement.name].Enqueue(go);
        RearrangeSiblings(go);

        return go;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"> 정렬하고 싶은 자식</param>
    private void RearrangeSiblings(GameObject obj)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == obj.name)
            {
                obj.transform.SetSiblingIndex(i);
                return;
            }
        }
        obj.transform.SetAsLastSibling();
    }
}
