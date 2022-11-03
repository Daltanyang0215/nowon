using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnits : MonoBehaviour
{
    public float length;
    public event Action OnReachedToEnd;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.back * 2.0f * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("MapEnd"))
        {
            OnReachedToEnd?.Invoke();
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStates newState)
    {
        enabled = newState == GameStates.Play;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
    }
}
