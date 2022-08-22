using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Box : MonoBehaviour
{
    [SerializeField] private GameObject _spwanBox; // box asset
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private float _spwanRange;
    [SerializeField] private float _spwangheiht;

    [SerializeField] private int _spwanCount;

    // Start is called before the first frame update
    void Start()
    {
        SpwanBox();
        InvokeRepeating("ResetBox", 5f,5f);
    }

    void SpwanBox()
    {
        for (int i = 0; i < _spwanCount; i++)
        {
            GameObject addblock = Instantiate(_spwanBox, SpwanRandomRange(), Quaternion.identity, _parentTransform);
        }
    }

    void ResetBox()
    {
        for (int i = 0; i < _parentTransform.transform.childCount; i++)
        {
            if (!_parentTransform.transform.GetChild(i).gameObject.activeSelf)
            {
                _parentTransform.transform.GetChild(i).gameObject.transform.position = SpwanRandomRange();
                _parentTransform.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private Vector3 SpwanRandomRange()
    {
        return new Vector3(Random.Range(-_spwanRange, _spwanRange), Random.Range(3, _spwangheiht), Random.Range(-_spwanRange, _spwanRange));
    }
}
