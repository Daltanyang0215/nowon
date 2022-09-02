using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string _sceneNameToMove;
    private void Start()
    {
        StartCoroutine(E_CheckSceneAndPutPlayerHere());
    }

    IEnumerator E_CheckSceneAndPutPlayerHere()
    {
        yield return new WaitUntil(()=> SceneInformation.isSceneLoaded);
        if(SceneInformation.oldSceneName == _sceneNameToMove)
            Player.Instance.transform.position = transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && Input.GetKeyDown(KeyCode.UpArrow))
            {
                SceneMover.MoveTo(_sceneNameToMove);
            }
        }
    }
}
